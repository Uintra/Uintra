import { Component, OnInit, HostListener } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { MqService } from 'src/app/shared/services/general/mq.service';
import { IULink } from "src/app/shared/interfaces/general.interface";

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

  constructor(private http: HttpClient, private mq: MqService) { }

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
  }

  openLeftNav() {
    document.body.classList.add("nav--open")
  }
}
