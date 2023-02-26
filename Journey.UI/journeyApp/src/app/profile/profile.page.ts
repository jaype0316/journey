import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { BooleanValueAccessor, ToastController } from '@ionic/angular';
import { Observable, of, Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CommonService } from '../services/common.service';
import { StorageService } from '../services/storage.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
})
export class ProfilePage implements OnInit {

  defaultTheme:string = 'light';
  profile:any;
  profileJson: string = null;
  isBusy: boolean;
  isEditMode: boolean = false;
  isChangingAvatar: boolean = false;
  avatarImage:any;
  subscriptions:Subscription[];

  constructor(private http:HttpClient, private storage: StorageService, private rendered: Renderer2, 
      private toastr: ToastController, private commonService:CommonService) { 
    this.subscriptions = new Array();
  }

  ngOnInit() {
  }

  ionViewDidEnter(){
    this.isBusy = true;
    const profileRequest = this.http.get(environment.journeyApi + "Account/UserProfile").subscribe(up =>{
      this.profile = up;
      console.log('profile == ', this.profile);
    }, (err) => {}, () => { this.isBusy = false;});
    this.subscriptions.push(profileRequest);
  }

  themeSelectionChanged(e){
    this.defaultTheme = e.detail.value;
    this.storage.save('theme', this.defaultTheme);
    this.rendered.setAttribute(document.body, 'color-theme', this.defaultTheme);
  }

  avatarChanged(files){
    this.isChangingAvatar = (this.avatarImage == null || this.avatarImage.fileName != files[0].name);
    this.clickEdit();
    this.avatarImage = files[0];
  }

  private saveAvatarImage():Observable<any>{
    if(!this.isChangingAvatar){
      return of();
    }

    let formData = new FormData();
    formData.append("file", this.avatarImage, this.avatarImage.name);

    return this.http.post(environment.journeyApi + "Account/ProfileImage", formData);
  }

  clickEdit(){
    this.isEditMode = true;
  }

  clickSave(){
    this.isEditMode = false;
    const avatarUpdateRequest = this.saveAvatarImage().subscribe(blobUri => {
      this.profile.avatarUrl = blobUri || this.profile.avatarUrl;
        this.commonService.say(this.profile.avatarUrl);
        const profileUpdateRequest = this.http.post(environment.journeyApi + "Account/Update", this.profile).subscribe(response => {  
          this.showToast('Save Successful');
        });
      this.subscriptions.push(profileUpdateRequest);
    });  
    this.subscriptions.push(avatarUpdateRequest);    
  }

  private reset(){
    this.isBusy = false;
    this.isEditMode = false;
    this.isChangingAvatar =false;
  }
  
  private async showToast(message:string){
    const toast = await this.toastr.create({
      message:message,
      duration: 2000
    });
    toast.present();
  }

  ionViewWillLeave(){
    this.reset();
    this.subscriptions.forEach(e => e.unsubscribe());
  }

}
