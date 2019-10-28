import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { config } from '../app.config';
import { Observable } from 'rxjs';


export interface INavigationItem {
  title: string;
  url: string;
  isActive?: boolean;
}
@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  constructor(
    private http: HttpClient
  ) { }

  getTopNavigation(): Observable<INavigationItem[]>
  {
    return this.http.get<INavigationItem[]>(`${config.api}/navigation/getTopNavigation`);
  }
}
