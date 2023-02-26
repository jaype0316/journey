import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  private subject: Subject<string> =new Subject<string>();

  constructor() { }

  public say(message:string){
    this.subject.next(message);
  }

  public hear(): Observable<string> {
    return this.subject.asObservable();
  }
}
