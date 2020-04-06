import { Injectable } from "@angular/core";
import { CookieService } from "ngx-cookie-service";
import { Subject } from 'rxjs';

@Injectable({
  providedIn: "root"
})
export class CentralFeedFiltersService {
  readonly openingStateProperty = "filters-state-opening";
  readonly tabStateProperty = "filters-state-tab";
  readonly filteringStateProperty = "filters-state-filtering";
  filter = new Subject<number>();

  constructor(private cookieService: CookieService) {}

  setOpeningState(data) {
    this.cookieService.set(this.openingStateProperty, JSON.stringify(data));
  }
  setTabState(data) {
    this.cookieService.set(this.tabStateProperty, JSON.stringify(data,));
  }
  setFilteringState(data) {
    this.cookieService.set(this.filteringStateProperty, JSON.stringify(data));
  }

  getOpeningState() {
    const cookieData = this.cookieService.get(this.openingStateProperty);
    return cookieData ? JSON.parse(cookieData) : false;
  }
  getTabState() {
    const cookieData = this.cookieService.get(this.tabStateProperty);
    return cookieData ? JSON.parse(cookieData) : null;
  }
  getFilteringState() {
    const cookieData = this.cookieService.get(this.filteringStateProperty);
    return cookieData ? JSON.parse(cookieData) : null;
  }
  changeFilter(val: number) {
    this.filter.next(val);
  }
}
