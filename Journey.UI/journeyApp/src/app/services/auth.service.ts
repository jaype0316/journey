import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { shareReplay } from 'rxjs/operators';
import { StorageService } from './storage.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient, private storage:StorageService, private jwtHelper:JwtHelperService) { }

  login(email:string, password:string){
    const loginRequest = this.http.post(environment.journeyApi + 'Account/Login', {email, password}).pipe(shareReplay());
    loginRequest.subscribe((response) =>{this.setSession(response)});

    return loginRequest;
  }

  private getAuthToken():string {
    return this.storage.get('jwt_token');
  }

  private setSession(authResponse){
    //this.storage.setItem('expires_at', authResponse.expiresAt);
    this.storage.save('jwt_token', authResponse.token);
  }

  logout(){
    const token = this.getAuthToken();
    const logoutRequest = this.http.post(environment.journeyApi + "Account/Logout", token);

    logoutRequest.subscribe(resp =>{
      this.storage.remove("jwt_token");
    });

    return logoutRequest;
  }

  isAuthenticated(){
    const token = this.storage.get("jwt_token");
    if(token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  isLoggedIn() {
    // let expiration = this.storage.getItem('expires_at');
    // let now = new Date(new Date().toUTCString());
    // const expiresAt = new Date(JSON.parse(expiration));
    // return expiresAt > now;
    var token = this.storage.get("jwt_token");
    return token != null;
  }
}
