import { Component, OnInit, HostListener, ElementRef } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { MqService } from 'src/app/shared/services/general/mq.service';
import { IULink } from "src/app/shared/interfaces/general.interface";
import { HeaderService } from "src/app/shared/services/general/header.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {
  @HostListener("window:resize", ["$event"])
  getScreenSize(event?) {
    this.deviceWidth = window.innerWidth;
    this.isDesktop = this.mq.isLaptop(this.deviceWidth);
  }

  deviceWidth: number;
  isDesktop: boolean;
  userListPage: IULink;

  constructor(
    private http: HttpClient,
    private mq: MqService,
    private headerService: HeaderService,
    private elRef: ElementRef<HTMLElement>)
  { }

  get isDesktopGeter() {
    return this.isDesktop;
  }

  ngOnInit() {
    this.deviceWidth = window.innerWidth;
    this.isDesktop = this.mq.isLaptop(this.deviceWidth);

    this.http.get<IULink>('/ubaseline/api/IntranetNavigation/UserList')
    .subscribe(res => {
      this.userListPage = res;
    });

    this.mq.mobileDesktop(
      () => {this.syncHeaderHeight()},
      () => {this.syncHeaderHeight()}
    );
  }
  ngAfterViewInit()
  {
    this.syncHeaderHeight();
  }

  openLeftNav(event) {
    event.preventDefault();
    document.body.classList.add("nav--open")
  }

  private syncHeaderHeight()
  {
    this.headerService.height = this.elRef.nativeElement.offsetHeight;
  }
}
