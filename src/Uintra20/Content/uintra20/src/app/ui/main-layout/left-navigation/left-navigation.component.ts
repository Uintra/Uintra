import { Component, OnInit } from "@angular/core";
import {
  LeftNavigationService,
  INavigationItem,
  INavigationData
} from "./left-navigation.service";
import { map } from "rxjs/operators";
import { CookieService } from "ngx-cookie-service";

@Component({
  selector: "app-left-navigation",
  templateUrl: "./left-navigation.component.html",
  styleUrls: ["./left-navigation.component.less"]
})
export class LeftNavigationComponent implements OnInit {
  navigationItems: INavigationItem[];
  openingState: object = {};

  constructor(
    private leftNavigationService: LeftNavigationService,
    private cookieService: CookieService
  ) {}

  ngOnInit() {
    this.leftNavigationService
      .getNavigation()
      .pipe(
        map(r => this.correctNestingLevel(r)),
        map(r => this.setOpenProperties(r))
      )
      .subscribe((r: INavigationItem[]) => {
        this.navigationItems = r;
      });
  }

  onToggleItem(item: INavigationItem) {
    this.addToOpeningState(item);
    item.isSelected = !item.isSelected;
  }

  addToOpeningState(item) {
    this.openingState[item.id] = !item.isSelected;
    this.cookieService.set(
      "nav-opening-state",
      JSON.stringify(this.openingState)
    );
  }

  correctNestingLevel(data) {
    return data.menuItems.map(item => {
      item.level = 0;
      return item;
    });
  }

  setOpenProperties(data) {
    const cookieData = this.cookieService.get("nav-opening-state");
    this.openingState = JSON.parse(cookieData);
    this.checkNavigationItem(data);
    return data;
  }

  checkNavigationItem(data) {
    return data.map(item => {
      if (this.openingState.hasOwnProperty(item.id)) {
        item.isSelected = this.openingState[item.id];
      }
      if (item.children.length) {
        this.checkNavigationItem(item.children);
      }
    });
  }

  getNestingPadding(level) {
    return level ? { paddingLeft: level * 10 + "px" } : {};
  }
}
