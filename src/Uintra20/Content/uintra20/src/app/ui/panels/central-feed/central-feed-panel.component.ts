import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ICentralFeedPanel } from './central-feed-panel.interface';
import { UmbracoFlatPropertyModel } from '@ubaseline/next';

// interface IFilterTab {
//   type: number;
//   isActive: boolean;
//   links: string;
//   title: string;
//   filters: object;
// }

@Component({
  selector: 'central-feed-panel',
  templateUrl: './central-feed-panel.html',
  styleUrls: ['./central-feed-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CentralFeedPanel implements OnInit{
  data: ICentralFeedPanel;

  tabs: Array<UmbracoFlatPropertyModel> = null;
  selectedTab: UmbracoFlatPropertyModel = null;
  selectTabFilters: Array<any>;
  selectedTabType: number;

  ngOnInit() {
    this.tabs = Object.values(this.data.tabs.get());
    this.setInitValues();
  }

  setInitValues() {
    this.selectedTab = this.tabs.find(tab => tab.get().isActive);
    this.selectedTabType = this.selectedTab.get().type.get();
    this.selectTabFilters = this.getTabFilters();
  }

  setSelectedTab(event) {
    this.selectedTabType = event;
    this.selectedTab = this.tabs.find(tab => tab.get().type.get() === event);
    this.selectTabFilters = this.getTabFilters();
  }

  clearFilters() {
    this.setInitValues();
    this.selectTabFilters = this.selectTabFilters
      .map(filter => ({ ...filter, isActive: false }));
  }

  getTabFilters() {
    const filters = Object.values(JSON.parse(JSON.stringify(this.selectedTab.get().filters.get())));

    return filters.map( filter => ({
      title: filter,
      isActive: false
      })
    );
  }
}
