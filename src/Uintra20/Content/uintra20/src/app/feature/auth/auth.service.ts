import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginModel } from '../login/models/login.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiRoute = 'api/auth/';

  constructor(private httpClient: HttpClient) { }

  public login(body: LoginModel) {
    return this.httpClient.post(`${this.apiRoute}login`, body);
  }

  public logout(): Observable<any> {
    return this.httpClient.post(`${this.apiRoute}logout`, null);
  }
}
