import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginModel } from 'src/app/feature/login/models/login.model';
import { BehaviorSubject } from 'rxjs';
import { LoginResult } from '../models/login-result.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private state$ = new BehaviorSubject(null);

  constructor(
    private httpClient: HttpClient) {
  }

  async login(body: LoginModel) {
    const result = await this.httpClient.post<LoginResult>('api/auth/login', body).toPromise();
    if (result) {
      return this.state$.next({ loginResult: result });
    } else {
      return this.state$.next(null);
    }
  }

  getState() {
    return this.state$;
  }
}
