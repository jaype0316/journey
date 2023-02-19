import { HttpClient } from '@angular/common/http';
import { Component, OnInit, SimpleChanges } from '@angular/core';
import { ToastController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Tag } from '../models/tag';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.page.html',
  styleUrls: ['./tags.page.scss'],
})
export class TagsPage implements OnInit {

  tagsSubscription:Subscription;
  isLoading:boolean;
  pristineTags:Tag[];
  tags:Tag[];
  isEditMode:boolean;

  constructor(private http:HttpClient, private toastr: ToastController) { 
    this.tags = new Array();
    this.pristineTags = new Array();
  }

  ngOnInit() {
  }

  ionViewDidEnter(){
    this.getTags();
  }

  private getTags(){
    this.isLoading = true;
    this.tagsSubscription = this.http.get(environment.journeyApi + "tags").subscribe((tags:any[]) => {
      this.tags = tags;
      this.pristineTags = JSON.parse(JSON.stringify(tags));
      console.log('tags == ', this.tags);
    },(err) =>{}, () => {this.isLoading = false;}); 
  }

  clickEdit(){
    this.isEditMode = true;
  }

  clickAddTag(){
    this.isEditMode = true;
    this.tags.push(new Tag("", "", false));
  }


  clickDeleteTag(tag:Tag){
    let index = this.tags.indexOf(tag);
    this.tags.splice(index);
  }

  clickSave(){
    console.log(this.tags);
    if(JSON.stringify(this.tags) == JSON.stringify(this.pristineTags)){
      console.log('no changes == !!');
      //no changes
      return;
    }
    this.http.post(environment.journeyApi + "tags", this.tags).subscribe((response:any) =>{
      this.showToast('Save successful');
      this.getTags();
      this.reset();
    });
  }

  private reset(){
    this.isEditMode = false;
  }

  ngOnChanges(changes:SimpleChanges){
    console.log('ngOnChanges fired == ', changes);
  }

  async showToast(message:string){
    const toast = await this.toastr.create({
      message:message,
      duration: 2000
    });
    toast.present();
  }

  ionViewWillLeave(){
    this.tagsSubscription.unsubscribe();
  }

}
