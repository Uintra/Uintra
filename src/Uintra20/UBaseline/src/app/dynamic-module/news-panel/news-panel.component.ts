import { Component, Input, HostBinding, OnInit } from '@angular/core';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { IPanelSettings } from 'src/app/shared/interface/panel-settings';
import { IPictureData } from 'src/app/shared/components/picture/picture.component';
import { IButtonData } from 'src/app/shared/components/button/button.component';
import { UrlType } from 'src/app/shared/enum/url-type';
import { resolveThemeCssClass } from '../lib/helper/panel-settings';
import { NewsOverviewService, INewsResponseModel } from './service/news-overview.service';
import { Observable } from 'rxjs';


export interface INewsPanelData {
  title: IUProperty<string>;
  description?: IUProperty<string>;
  seeAllUrl: string;
  panelSettings: IPanelSettings;
  previewsCount: IUProperty<number>;
}

export interface INewsPanelItem {
  title: string;
  description: string;
  previewImage: IPictureData;
  publishDate: string;
  publishDateString: string;
  url: string;
}

@Component({
  selector: 'app-news-panel',
  templateUrl: './news-panel.component.html',
  styleUrls: ['./news-panel.component.less']
})
export class NewsPanelComponent implements OnInit {
  @Input() data: Partial<INewsPanelData>;
  private newsList: Observable<INewsResponseModel>;
  newsArray: INewsPanelItem[];

  @HostBinding('class') get hostClasses() {return resolveThemeCssClass(this.data.panelSettings)}

  constructor(
    private newsService: NewsOverviewService
  ) { }

  ngOnInit() {
    this.newsList = this.getNews();

    this.newsList.subscribe(response => {
      this.newsArray = response.items;
    });
  }

  getNews() {
    const DEFAULT_NEWS_COUNT = 3;
    const newsCount = this.data.previewsCount ? this.data.previewsCount.value : DEFAULT_NEWS_COUNT;

    const initialSettings = {
      year: null,
      index: 0,
      step: newsCount
    };

    return this.newsService.getNews(initialSettings);
  }

}
