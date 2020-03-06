import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { CookieService } from "ngx-cookie-service";
import { map } from "rxjs/operators";
import { INavigationItem, INavigationData } from "./left-navigation.interface";

@Injectable({
  providedIn: "root"
})
export class LeftNavigationService {
  readonly api = "ubaseline/api/IntranetNavigation";
  readonly openingStateProperty = "nav-opening-state";
  openingState: object = {};

  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.updateOpeningState();
  }

  setOpeningState(item: INavigationItem) {
    this.openingState = { ...this.openingState, [item.id]: !item.isSelected };

    this.cookieService.set(this.openingStateProperty, JSON.stringify(this.openingState));
    this.updateOpeningState();
  }

  getNavigation(): Observable<INavigationData> {
    return this.http.get<INavigationData>(this.api + `/LeftNavigation`).pipe(
      map(r => this.correctNestingLevel(r)),
      map(r => this.setOpenProperties(r))
    );
  }

  private correctNestingLevel(data: INavigationData): INavigationData {
    data.menuItems = data.menuItems.map(item => {
      item.level = 0;
      return item;
    });

    return data;
  }

  private setOpenProperties(data: INavigationData): INavigationData {
    if (this.openingState) {
      data.menuItems = this.checkNavigationItem(data.menuItems);
    }
    return data;
  }

  private checkNavigationItem(data) {
    return data.map(item => {
      if (this.openingState.hasOwnProperty(item.id)) {
        item.isSelected = this.openingState[item.id];
      }
      if (item.children.length) {
        this.checkNavigationItem(item.children);
      }
      return item;
    });
  }

  private updateOpeningState() {
    const cookieData = this.cookieService.get(this.openingStateProperty);
    this.openingState = cookieData ? JSON.parse(cookieData) : [];
  }
}
