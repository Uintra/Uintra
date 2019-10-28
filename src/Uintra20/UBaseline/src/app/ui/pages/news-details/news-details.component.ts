import { Component } from '@angular/core';
import { AbstractPageComponent, IPageData } from 'src/app/shared/components/abstract-page/abstract-page.component';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { IPictureData } from 'src/app/shared/components/picture/picture.component';

interface INewsDetails extends IPageData {
  title: IUProperty<string>;
  description: IUProperty<string>;
  newsOverviewLink: string;
  previewImage: IUProperty<IPictureData>;
  publishDate: IUProperty<string>;
  publishDateString: string;
  showInOverview: IUProperty<boolean>;
  lastUpdatedDateString: string;
}

@Component({
  selector: 'app-news-details',
  templateUrl: './news-details.component.html',
  styleUrls: ['./news-details.component.less']
})
export class NewsDetailsComponent extends AbstractPageComponent {
  data:INewsDetails;
  defaultTitle = 'News details page';

  url: string = window.location.href;
}
