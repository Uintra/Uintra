import { Component, OnInit } from "@angular/core";
import {
  LeftNavigationService,
  INavigationItem,
  INavigationData
} from "./left-navigation.service";

@Component({
  selector: "app-left-navigation",
  templateUrl: "./left-navigation.component.html",
  styleUrls: ["./left-navigation.component.less"]
})
export class LeftNavigationComponent implements OnInit {
  navigationItems: INavigationItem[];

  constructor(private leftNavigationService: LeftNavigationService) {}

  ngOnInit() {
    this.leftNavigationService
      .getNavigation()
      .subscribe((r: INavigationData) => {
        this.navigationItems = r.menuItems;
      });
  }
}
