import { Component, OnInit, HostListener } from '@angular/core';
import { MqService } from 'src/app/shared/services/general/mq.service';

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

  constructor(private mq: MqService) { }

  get isDesktopGeter() {
    return this.isDesktop;
  }

  ngOnInit() {
    this.deviceWidth = window.innerWidth;
    this.isDesktop = this.mq.isLaptop(this.deviceWidth);
  }

  openLeftNav() {
    document.body.classList.add("nav--open")
  }
}
