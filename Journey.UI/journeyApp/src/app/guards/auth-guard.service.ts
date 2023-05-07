import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { StorageService } from '../services/storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticatedGuard implements CanActivate {

  constructor(private router:Router,  private storage:StorageService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    // const token = this.storage.get("jwt_token");
    // if(token && !this.jwtHelper.isTokenExpired(token)){
    //   return true;
    // }

    // this.storage.remove('jwt_token');
    // this.router.navigate(["authenticate"]);
    // return false;
    return true;
  }
}
