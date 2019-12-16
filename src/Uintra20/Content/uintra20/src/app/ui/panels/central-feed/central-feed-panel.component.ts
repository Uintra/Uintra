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
  selectTabFilters: Array<any>;
  selectedTabType: number;
  feed: Array<any> = [];
  currentPage: number = 1;
  isFeedLoading: boolean = false;

  constructor(private publicationsService: PublicationsService) {}

  ngOnInit() {
    this.tabs = Object.values(this.data.tabs.get());
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

    this.isFeedLoading = true;
    this.publicationsService.getPublications(data).then(response => {
      this.feed = this.feed.concat(response['feed']);
    }).catch(error => {

    }).finally(() => {
      this.isFeedLoading = false;
    });
  }

  onLoadMore() {
    this.currentPage += 1;
    this.getPublications();
  }

  onScroll() {
    this.onLoadMore();
  }

  selectFilters({ selectedTabType, selectTabFilters }) {
    this.selectTabFilters = selectTabFilters;
    this.selectedTabType = selectedTabType;
    this.feed = [];
    this.getPublications();
  }
}
