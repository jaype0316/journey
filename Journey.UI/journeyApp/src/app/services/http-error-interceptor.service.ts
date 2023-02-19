import { HttpClient, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastController } from '@ionic/angular';
import { throwError } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor {

  constructor(private toast: ToastController, private router:Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((response) => {
        console.log('error is intercept')
        console.error(response);
        //let specific call sites handle 400's, for everything else show toast
        if(response.status === 401){
          this.router.navigate(['authenticate']);// this.router.navigate(['tabs/chapter',chapter.pk]);
        }
        if(response.status !== 400){
          this.showToast('An error occurred');
        }
        return throwError(response.error);
      })
    )
  }

  async showToast(message:string){
    const toast = await this.toast.create({
      message:message,
      duration: 2000,
      color:'danger'
    });
    toast.present();
  }
}
