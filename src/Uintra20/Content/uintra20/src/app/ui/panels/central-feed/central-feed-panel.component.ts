import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ICentralFeedPanel } from './central-feed-panel.interface';

@Component({
  selector: 'central-feed-panel',
  templateUrl: './central-feed-panel.html',
  styleUrls: ['./central-feed-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CentralFeedPanel implements OnInit{
  data: ICentralFeedPanel;

  tabs = [];
  selectedTab = null;

  ngOnInit() {
    this.tabs = Object.values(this.data.tabs.get());

    const selectedTab = this.tabs.find(tab => tab.get().isActive);
    this.selectedTab = selectedTab.get().type.get();
  }
}
