import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UmbracoFlatPropertyModel } from '@ubaseline/next';

@Component({
  selector: 'app-central-feed-filters',
  templateUrl: './central-feed-filters.component.html',
  styleUrls: ['./central-feed-filters.component.less']
})
export class CentralFeedFiltersComponent implements OnInit {
  @Input() tabs: Array<UmbracoFlatPropertyModel>;
  @Output() selectFilters = new EventEmitter();

  selectedTab: UmbracoFlatPropertyModel = null;
  selectTabFilters: Array<any>;
  selectedTabType: number;

  constructor() { }

  ngOnInit() {
    this.setInitValues();
    this.selectFilters.emit({ selectedTabType: this.selectedTabType, selectTabFilters: this.selectTabFilters } );
  }

  setSelectedTab(event) {
    this.selectedTabType = event;
    this.selectedTab = this.tabs.find(tab => tab.get().type.get() === event);
    this.selectTabFilters = this.getTabFilters();
    this.selectFilters.emit({ selectedTabType: this.selectedTabType, selectTabFilters: this.selectTabFilters } );
  }

  changeFilters() {
    this.selectFilters.emit({ selectedTabType: this.selectedTabType, selectTabFilters: this.selectTabFilters } );
  }

  clearFilters() {
    this.setInitValues();
    this.selectTabFilters = this.selectTabFilters
      .map(filter => ({ ...filter, isActive: false }));
  }

  setInitValues() {
    this.selectedTab = this.tabs.find(tab => tab.get().isActive);
    this.selectedTabType = this.selectedTab.get().type.get();
    this.selectTabFilters = this.getTabFilters();
  }

  getTabFilters() {
    return Object.values(JSON.parse(JSON.stringify(this.selectedTab.get().filters.get())));
  }
}
