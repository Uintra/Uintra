import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginModel } from 'src/app/feature/login/models/login.model';
import { Observable } from 'rxjs';
import { LoginResult } from '../models/login-result.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(
    private httpClient: HttpClient) {
  }

  login(body: LoginModel): Observable<LoginResult> {
    return this.httpClient.post<LoginResult>('api/auth/login', body);
  }
}
