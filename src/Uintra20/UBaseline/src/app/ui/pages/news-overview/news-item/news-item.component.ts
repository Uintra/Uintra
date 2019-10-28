import { Component, Input } from '@angular/core';
import { INewsItem } from 'src/app/service/news.service';

@Component({
  selector: 'news-item',
  templateUrl: './news-item.component.html',
  styleUrls: ['./news-item.component.less']
})
export class NewsItemComponent {
  @Input() news: INewsItem;

  constructor() { }
}
