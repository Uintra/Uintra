import { Component, OnInit, Injector } from '@angular/core';
import { Subject } from 'rxjs';
import { PageResolverService } from 'src/app/service/page-resolver.service';
import { Title } from '@angular/platform-browser';
import { SEOService, IPageMetaData } from 'src/app/service/seo.service';
import { Location } from '@angular/common';
import { IUProperty } from '../../interface/umbraco-property';
import { SiteSettingsService } from 'src/app/service/site-settings.service';
import { ISiteSettings } from '../../interface/site-settings';
import get from 'lodash/get';

export interface IPageData {
  title: IUProperty<string>;
  url: string;
  metaData: IPageMetaData;
  panels: IUProperty<any[]>;
}
@Component({
  selector: 'app-abstract-page',
  template: '',
  styles: ['']
})
export class AbstractPageComponent implements OnInit {

  data: IPageData = null;
  protected defaultTitle = '';
  protected siteSettings: ISiteSettings;
  protected alive$ = new Subject;

  constructor(
    protected pageResolverService: PageResolverService,
    protected titleService: Title,
    protected seoService: SEOService,
    protected location: Location,
    protected injector: Injector,
    protected siteSettingsService: SiteSettingsService
  ) { }

  async ngOnInit()
  {
    this.data = await this.pageResolverService.byPath(this.location.path()) as IPageData;
    this.siteSettings = await this.siteSettingsService.getSiteSettings();

    this.titleService.setTitle(this.resolvePageTitle(this.data, this.siteSettings));

    if (this.data !== null)
    {
      this.seoService.addTags(this.data.metaData);
      this.data.url && this.seoService.addCanonicalURL(this.data.url);
    }
  }

  ngOnDestroy()
  {
    this.alive$.next();
    this.alive$.complete();
  }

  private resolvePageTitle(data: IPageData, settings: ISiteSettings): string
  {
    let first = get(data, 'metaData.metaTitle.value') || get(data, 'title.value') || '';
    let last = get(settings,'headerSettings.siteTitle');
    let separator = settings.pageTitleSeparator ? ` ${settings.pageTitleSeparator} ` : ' | ';

    return  [first, last].filter(i => i).join(separator);
  }
}