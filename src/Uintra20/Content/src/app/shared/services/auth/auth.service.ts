import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ILoginPage } from 'src/app/ui/pages/login/login-page.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiRoute = 'api/auth/';

  constructor(private httpClient: HttpClient) { }

  public login(body: ILoginPage) {
    return this.httpClient.post(`${this.apiRoute}login`, body);
  }

  public logout(): Observable<any> {
    return this.httpClient.post(`${this.apiRoute}logout`, null);
  }
}
