import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { INewsPanelItem } from '../../news-panel.component';

@Component({
  selector: 'app-news-panel-item',
  templateUrl: './news-panel-item.component.html',
  styleUrls: ['./news-panel-item.component.less']
})
export class NewsPanelItemComponent implements OnInit {
  @Input() data: INewsPanelItem;
  @HostBinding('class') get className() { return 'news-panel-item'};

  constructor() { }

  ngOnInit() {

  }

}
