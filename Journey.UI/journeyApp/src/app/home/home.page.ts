import { Component, OnInit } from '@angular/core';
import { Book } from '../models/book';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { ToastController } from '@ionic/angular';
import { AuthService } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Browser } from '@capacitor/browser';
import { mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss']
})
export class HomePage implements OnInit {
  
  isEditMode:boolean = false;
  book: Book;
  logoFile:any;
  isLoading:boolean;
  profile:any;
  isChangingImage:boolean;

  BUCKET_URI: string = 'https://journey-logos.s3.us-east-2.amazonaws.com/';
  FALLBACK_COVER:string = 'https://journey-logos.s3.us-east-2.amazonaws.com/fallback-cover.jpg';
  
  constructor(private http:HttpClient, private toastr: ToastController, public auth: AuthService, private router:Router) { 
  }

  ngOnInit() {
    //re-direct user to login if not authenticated
    this.isLoading = true;
    this.auth.isAuthenticated$.subscribe((isAuthenticated) => {
      if(!isAuthenticated) {
        this.auth
          .buildAuthorizeUrl()
          .pipe(mergeMap((url) => Browser.open({ url, windowName: '_self' })))
          .subscribe();

        // this.auth.loginWithRedirect({appState:{
        //   target:window.top.location.pathname
        // }});
        return;
      }

    this.auth.user$.subscribe(
      (profile) => {
        this.profile = profile;
      });

      this.bootstrap().subscribe((book) => {
        this.book = book;
      },(err) =>{}, () => {this.isLoading = false;}); 

    });
  }

  isPageValid(){
    if(this.book == null) 
      return false;

    if(this.book.title && this.book.about)
      return true;

      return false;
  }

  async getBook(){
    await this.http.get(environment.journeyApi + 'Book').subscribe((book:Book) =>{
      this.isEditMode = book == null;
      this.book = book || new Book();
      this.isLoading = false;

      console.log('got book == ', this.book);
    });
  }

  private bootstrap():Observable<any> {
    return this.http.get(environment.journeyApi + 'Account/bootstrap');
  }

  private saveCoverImage():Observable<any>{
    let formData = new FormData();
    formData.append("file", this.logoFile, this.logoFile.name);

    return this.http.post(environment.journeyApi + "Book/upload-logo", formData);
  }

//this.book.logoKey = blobKey;
  async clickDone(){
    this.isEditMode = false;
    console.log(this.book);

    this.http.post(environment.journeyApi + "Book/save", this.book).subscribe((book:Book) => {
      this.book = book;
      this.showToast('saved');
    }, (error:any) => {
      this.showToast('an error ocurred.');
    }, () => {

    });
  }

  clickEdit(){
    this.isEditMode = true;
  }

  async showToast(message:string){
    const toast = await this.toastr.create({
      message:message,
      duration: 2000
    });
    toast.present();
  }

  clickNewEntry(){
    //<a [routerLink]="['/tabs/chapters/new']">
    this.router.navigate(['/tabs/chapters/new']);
  }

  onFileChange($event){
    this.isChangingImage = true;
    const didCoverImageChange = (this.logoFile == null || this.logoFile.fileName != $event.target.files[0].fileName);
    this.logoFile = $event.target.files[0];
    if(didCoverImageChange){
      this.saveCoverImage().subscribe((blobKey:string) =>{
          this.book.logoKey = blobKey;
      },(err) => {}, () => {this.isChangingImage = false;})
    }
  }
}
