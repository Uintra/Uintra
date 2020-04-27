import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ActivationStart, ChildActivationStart } from '@angular/router';
import { LoginPage } from './ui/pages/login/login-page.component';
import { TranslateService } from '@ngx-translate/core';
import { SiteSettingsService, ISiteSettings } from '@ubaseline/next';
import { IApplication } from './shared/interfaces/components/application/iapplication.interface';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {

  public title = 'uintra20';
  public isLoginPage = true;
  public hasLeftLoginPage = true;
  public hasPanels = false;
  public siteSettings: ISiteSettings;

  public data: IApplication;
  public latestActivities: any;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private translateService: TranslateService,
    private siteSettingsService: SiteSettingsService,
  ) {
    this.route.data.subscribe((data: IApplication) => {
      this.data = data;
      this.hasPanels = data && data.panels && data.panels;
    });

    this.router.events.subscribe(val => {
      if (val instanceof ActivationStart) {
        if (val.snapshot.component) {
          this.isLoginPage = val.snapshot.component === LoginPage;
          if (this.isLoginPage) {
            this.hasLeftLoginPage = false;
            this.siteSettingsService.getSiteSettings().then((res: ISiteSettings) => {
              this.siteSettings = res;
            });
            document.title = `Login ${this.siteSettings.pageTitleSeparator} ${this.siteSettings.siteTitle}`;
          }
        }
      }
      if (!this.isLoginPage && val instanceof ChildActivationStart) {
        this.hasLeftLoginPage = true;
      }
    });
  }

  public ngOnInit(): void {
    this.translateService.use('');
  }

  public closeLeftNav(): void {
    document.body.classList.remove('nav--open');
  }
}

