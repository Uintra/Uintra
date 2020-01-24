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
}

@Injectable({
  providedIn: 'root'
})
export class LeftNavigationService {
  readonly api = 'ubaseline/api/IntranetNavigation';

  constructor(private http: HttpClient) {}

  getNavigation(): Observable<INavigationData> {
    // return this.http.get<INavigationData>(this.api + `/LeftNavigation`);
    return new Observable(subscriber => {
      subscriber.next({"menuItems":[{"id":1301,"name":"Central Feed test","url":"/central-feed-test/","isActive":false,"isHomePage":false,"isClickable":false,"isHeading":false,"children":[]},{"id":2511,"name":"Heading 1","url":"/heading-1/","isActive":false,"isHomePage":false,"isClickable":false,"isHeading":false,"children":[{"id":2512,"name":"test page","url":"/heading-1/test-page/","isActive":false,"isHomePage":false,"isClickable":false,"isHeading":false,"children":[]}]}]});
    });
  }
}
