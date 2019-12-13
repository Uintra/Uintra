import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ICentralFeedPanel } from './central-feed-panel.interface';
import { UmbracoFlatPropertyModel } from '@ubaseline/next';
import { PublicationsService, IFeedListRequest } from './helpers/publications.service';

// interface IFilterTab {
//   type: number;
//   isActive: boolean;
//   links: string;
//   title: string;
//   filters: object;
// }
// interface IFeedData {
//   activity: {
//     creatorId: string;
//     description: string;
//     endPinDate: string;
//     groupId: string;
//     isHidden: string;
//     isPinned: string;
//     linkPreviewId: string;
//     ownerId: string;
//     publishDate: string;
//     title: string;
//     umbracoCreatorId: string;
//   };
//   controllerName: string;
//   options: {
//     isReadOnly: false
//     links: {
//       create: string;
//       details: string;
//       detailsNoId: string;
//       edit: string;
//       feed: string;
//       overview: string;
//       owner: string;
//     }
//   }
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
  feed: Array<any> = [];
  currentPage: number = 1;

  constructor(private publicationsService: PublicationsService) {}

  ngOnInit() {
    this.tabs = Object.values(this.data.tabs.get());
    this.setInitValues();
    this.getPublications();
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
    this.getPublications();
  }

  changeFilters() {
    this.getPublications();
  }

  clearFilters() {
    this.setInitValues();
    this.selectTabFilters = this.selectTabFilters
      .map(filter => ({ ...filter, isActive: false }));
  }

  getPublications() {
    const FilterState = {};

    this.selectTabFilters.forEach(filter => {
      FilterState[filter.key] = filter.isActive;
    });

    const data = {
      TypeId: this.selectedTabType,
      FilterState,
      Page: this.currentPage
    };

    this.publicationsService.getPublications(data).then(response => {
      this.feed = [...this.feed, response['feed']];
    }).catch(error => {

    });
  }

  getTabFilters() {
    const filters = Object.values(JSON.parse(JSON.stringify(this.selectedTab.get().filters.get())));

    return filters;
  }

  onLoadMore() {
    this.currentPage += 1;
    this.getPublications();
  }
}
