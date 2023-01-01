import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private storage:StorageService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const idToken = this.storage.get('jwt_token');
    if(idToken){
      const cloned = req.clone({
        headers:req.headers.set("Authorization","Bearer "+ idToken)
      });
      return next.handle(cloned);
    } else {
      return next.handle(req);
    }
  }
}
