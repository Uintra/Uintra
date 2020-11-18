import { ViewportScroller } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AppService } from './app.service';

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"],
})
export class AppComponent implements OnInit {
  public onlyAuthenticated: boolean;

  constructor(
    private translateService: TranslateService,
    private viewportScroller: ViewportScroller,
    private appService: AppService
  ) {}

  public ngOnInit(): void {
    this.translateService.use("");
    this.appService.getPageAccessTrigger().subscribe((access) => {
      this.onlyAuthenticated = !access;
    })
  }

  public closeLeftNav(): void {
    document.body.classList.remove("nav--open");
  }

  scrollToBlock(event) {
    event.preventDefault();
    let targetElement = document.querySelectorAll(
      event.currentTarget.getAttribute("href")
    )[0];

    targetElement.focus();
    targetElement.blur();

    this.viewportScroller.scrollToPosition([0, targetElement.offsetTop]);
  }
}
