import { Component, OnInit, ElementRef, ViewChild, AfterViewInit, ViewEncapsulation, HostListener } from "@angular/core";
import { INavigationItem, INavigationData } from "./left-navigation.interface";
import { LeftNavigationService } from "./left-navigation.service";
import SimpleScrollbar from "simple-scrollbar";
import { MqService } from 'src/app/shared/services/general/mq.service';
import { Router, NavigationEnd } from '@angular/router';
import { IGroupsData } from 'src/app/feature/specific/groups/groups.interface';
import { ISharedNavData } from './components/shared-links/shared-links.service';
import { IMyLink } from './components/my-links/my-links.service';

@Component({
  selector: "app-left-navigation",
  templateUrl: "./left-navigation.component.html",
  styleUrls: ["./left-navigation.component.less"]
})
export class LeftNavigationComponent implements OnInit, AfterViewInit {
  @ViewChild("wrapper", {static: false}) wrapperView: ElementRef;
  @HostListener("window:resize", ["$event"])
  getScreenSize(event?) {
    this.deviceWidth = window.innerWidth;
    this.isMobile = this.mq.mdDown(this.deviceWidth);
  }

  navigationItems: INavigationItem[];
  isMobile: boolean;
  deviceWidth: number;
  groupItems: IGroupsData;
  sharedLinks: ISharedNavData;
  myLinks: Array<IMyLink>;
  readonly PADDING_STEP = 10;

  constructor(
    private leftNavigationService: LeftNavigationService,
    private mq: MqService,
    private router: Router) {
      this.router.events.subscribe(val => {
        if (val instanceof NavigationEnd) {
          this.closeLeftNav();
        }
      })
    }

  get isNotDesktop() {
    return this.isMobile;
  }

  ngOnInit() {
    this.leftNavigationService
      .getNavigation()
      .subscribe((r: INavigationData) => {
        this.navigationItems = r.menuItems;
        this.groupItems = r.groupItems;
        this.sharedLinks = r.sharedLinks;
        this.myLinks = r.myLinks;
      });
    this.deviceWidth = window.innerWidth;
    this.isMobile = this.mq.mdDown(this.deviceWidth);
  }

  ngAfterViewInit(){
    SimpleScrollbar.initEl(this.wrapperView.nativeElement);
  }

  onToggleItem(item: INavigationItem) {
    this.leftNavigationService.setOpeningState(item);
    item.isSelected = !item.isSelected;
  }

  getNestingPadding(level: number) {
    return level ? { paddingLeft: level * this.PADDING_STEP + "px" } : {};
  }

  closeLeftNav() {
    document.body.classList.remove("nav--open")
  }
}
