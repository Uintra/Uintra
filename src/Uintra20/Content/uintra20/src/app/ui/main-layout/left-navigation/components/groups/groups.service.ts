import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { CookieService } from 'ngx-cookie-service';

export interface IGroupsData {
  groupPageItem: IGroupsLink;
  items: IGroupsLink[];
}

export interface IGroupsLink {
  title: string;
  link: {
    originalUrl: string;
    baseUrl: string;
    params: Array<object>;
  };
}

@Injectable({
  providedIn: "root"
})
export class GroupsService {
  readonly api = "ubaseline/api/Group";
  readonly openStateProperty = "nav-group-links-open";

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  getGroupsLinks(): Observable<IGroupsData> {
    return this.http.get<IGroupsData>(this.api + `/LeftNavigation`);
  }

  setOpenState(openState: boolean = false): void {
    this.cookieService.set(this.openStateProperty, openState.toString());
  }

  getOpenState(): boolean {
    const cookieData = this.cookieService.get(this.openStateProperty);
    return cookieData === "true";
  }
}
