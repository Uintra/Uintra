import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface INavigationData {
  menuItems: INavigationItem[];
}

export interface INavigationItem {
  id: number;
  name: string;
  url: string;
  isActive: boolean;
  isHomePage: boolean;
  isClickable: boolean;
  isHeading: boolean;
  children: INavigationItem[];

  isSelected: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class LeftNavigationService {
  readonly api = 'ubaseline/api/IntranetNavigation';

  constructor(private http: HttpClient) {}

  getNavigation(): Observable<INavigationData> {
    return this.http.get<INavigationData>(this.api + `/LeftNavigation`);
  }
}
