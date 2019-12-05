import { Component } from '@angular/core';
import { SiteSettingsService } from "./service/site-settings.service";
import { ISiteSettings } from './shared/interface/site-settings';
import { NavigationService, INavigationItem } from './service/navigation.service';
import { TranslateService } from '@ngx-translate/core';
import { DrawerService } from './dynamic-module/lib/drawer/service/drawer.service';
import { RestoreScrollService } from './service/restore-scroll.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  siteSettings: ISiteSettings;
  topNavigation: INavigationItem[];

  constructor(
    private siteSettingsService: SiteSettingsService,
    private navigationService: NavigationService,
    private translateService: TranslateService,
    public drawerService: DrawerService,
    private restoreScrollService: RestoreScrollService
  ) {}

  async ngOnInit()
  {
    this.restoreScrollService.watchAndRestore();
    this.translateService.use('');
    this.siteSettings = await this.siteSettingsService.getSiteSettings();
    this.topNavigation = await this.navigationService.getTopNavigation().toPromise();
  }
}
