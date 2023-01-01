import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';
import { throwError } from 'rxjs';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor {

  constructor(private toast: ToastController) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error) => {
        console.log('error is intercept')
        console.error(error);
        this.showToast('An error occurred');
        return throwError(error.message);
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
