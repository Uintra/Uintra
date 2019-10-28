import { Component } from '@angular/core';
import { AbstractPageComponent } from 'src/app/shared/components/abstract-page/abstract-page.component';

@Component({
  selector: 'app-news-overview-page',
  templateUrl: './news-overview-page.component.html',
  styleUrls: ['./news-overview-page.component.less']
})
export class NewsOverviewPageComponent extends AbstractPageComponent {
  defaultTitle = 'News overview';
}
