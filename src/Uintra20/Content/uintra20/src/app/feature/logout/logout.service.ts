import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {

  constructor(private httpClient: HttpClient) { }

  public logout(): Observable<any> {
    return this.httpClient.post('api/auth/logout', null);
  }
}
