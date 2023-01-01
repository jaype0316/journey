import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class StorageService {

  constructor() { }

  save(key:string, item:any){
    window.localStorage.setItem(key, JSON.stringify(item));
  }

  get(key:string){
    let value = window.localStorage.getItem(key);
    return JSON.parse(value) || null;
  }

  remove(key:string){
    window.localStorage.removeItem(key);
  }

}
