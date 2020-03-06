import { Component } from "@angular/core";
import { ActivatedRoute, Router, ActivationStart, ChildActivationStart } from "@angular/router";
import { LoginPage } from "./ui/pages/login/login-page.component";
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"]
})
export class AppComponent {
  title = "uintra20";

  isLoginPage: boolean = true;
  hasLeftLoginPage: boolean = true;
  hasPanels: boolean = false;

  data: any;
  latestActivities: any;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private translateService: TranslateService) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.hasPanels = data && data.panels && data.panels.get();
    });

    router.events.subscribe(val => {
      if (val instanceof ActivationStart) {
        if (val.snapshot.component) {
          this.isLoginPage = val.snapshot.component === LoginPage;
          if (this.isLoginPage) {
            this.hasLeftLoginPage = false;
          }
        }
      }
      if (!this.isLoginPage && val instanceof ChildActivationStart) {
        this.hasLeftLoginPage = true;
      }
    });
  }

  ngOnInit() {
    this.translateService.use('');
  }

  closeLeftNav() {
    document.body.classList.remove("nav--open")
  }
}
