import { Component, OnInit, ElementRef, ViewChild, AfterViewInit, ViewEncapsulation, HostListener } from "@angular/core";
import { INavigationItem } from "./left-navigation.interface";
import { LeftNavigationService } from "./left-navigation.service";
import SimpleScrollbar from "simple-scrollbar";
import { MqService } from 'src/app/services/general/mq.service';

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
  readonly PADDING_STEP = 10;

  constructor(
    private leftNavigationService: LeftNavigationService,
    private mq: MqService) {}

  get isNotDesktop() {
    return this.isMobile;
  }

  ngOnInit() {
    this.leftNavigationService
      .getNavigation()
      .subscribe((r: INavigationItem[]) => {
        this.navigationItems = r;
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
