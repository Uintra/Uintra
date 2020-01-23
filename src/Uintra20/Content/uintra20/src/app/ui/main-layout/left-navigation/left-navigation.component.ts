import { Component, OnInit } from "@angular/core";
import { INavigationItem } from "./left-navigation.interface";
import { LeftNavigationService } from "./left-navigation.service";

@Component({
  selector: "app-left-navigation",
  templateUrl: "./left-navigation.component.html",
  styleUrls: ["./left-navigation.component.less"]
})
export class LeftNavigationComponent implements OnInit {
  navigationItems: INavigationItem[];
  readonly PADDING_STEP = 10;

  constructor(private leftNavigationService: LeftNavigationService) {}

  ngOnInit() {
    this.leftNavigationService
      .getNavigation()
      .subscribe((r: INavigationItem[]) => {
        this.navigationItems = r;
      });
  }

  onToggleItem(item: INavigationItem) {
    this.leftNavigationService.setOpeningState(item);
    item.isSelected = !item.isSelected;
  }

  getNestingPadding(level: number) {
    return level ? { paddingLeft: level * this.PADDING_STEP + "px" } : {};
  }
}
