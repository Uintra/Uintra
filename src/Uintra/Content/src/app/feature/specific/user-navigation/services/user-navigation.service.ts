import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserNavigationService {

  private prefix = '/ubaseline/api/';

  constructor(private httpClient: HttpClient) { }

  public topNavigation = (): Observable<any> =>
    this.httpClient.get(`${this.prefix}intranetNavigation/topNavigation`)

  public redirect = (url: string): Observable<any> =>
    this.httpClient.post(url, null)
}
