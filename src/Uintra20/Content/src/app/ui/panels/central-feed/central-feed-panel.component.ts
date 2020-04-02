import { Component, ViewEncapsulation, OnInit, NgZone } from "@angular/core";
import { ICentralFeedPanel, IPublicationsResponse, IFilterState } from "./central-feed-panel.interface";
import { UmbracoFlatPropertyModel, IUmbracoProperty } from "@ubaseline/next";
import { PublicationsService } from "./helpers/publications.service";
import { SignalrService } from "src/app/shared/services/general/signalr.service";
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: "central-feed-panel",
  templateUrl: "./central-feed-panel.html",
  styleUrls: ["./central-feed-panel.less"],
  encapsulation: ViewEncapsulation.None
})
export class CentralFeedPanel implements OnInit {
  data: ICentralFeedPanel;
  tabs: Array<any> = null;
  selectTabFilters: Array<IFilterState>;
  selectedTabType: number;
  feed: Array<any> = [];
  currentPage = 1;
  isFeedLoading = false;
  isResponseFailed = false;
  isScrollDisabled = false;

  constructor(
    private publicationsService: PublicationsService,
    private socialService: ActivityService,
    private signalrService: SignalrService,
    private ngZone: NgZone,
    private translate: TranslateService,
  ) {}

  ngOnInit() {
    this.tabs = this.filtersBuilder();

    this.signalrService.getReloadFeedSubjects().subscribe(s => {
      this.reloadFeed();
    });
  }

  filtersBuilder() {
    let filtersFromServer = Object.values(this.data.tabs.get());

    // TODO: fix ubaselline next and remove it
    const allOption = new UmbracoFlatPropertyModel({
      type: "0",
      isActive: true,
      links: null,
      title: this.translate.instant('centralFeed.Filter.All.lnk'),
      filters: [
        {
          key: "ShowPinned",
          title: this.translate.instant('centralFeedList.ShowPinned.chkbx'),
          isActive: false
        }
      ]
    } as any);

    filtersFromServer.unshift(allOption);

    return filtersFromServer;
  }

  reloadFeed(): void {
    if (typeof window !== "undefined") {
      window.scrollTo(0, 0);
    }
    this.resetFeed();
    this.getPublications();
  }

  getPublications(): void {
    const FilterState = {};

    this.selectTabFilters.forEach(filter => {
      FilterState[filter.key] = !!filter.isActive;
    });
    const data = {
      TypeId: this.selectedTabType,
      FilterState,
      Page: this.currentPage,
      groupId: this.data.groupId.data.value
    };

    this.isFeedLoading = true;

    this.publicationsService
      .getPublications(data)
      .then((response: IPublicationsResponse) => {
        this.isScrollDisabled = response.feed.length === 0;
        this.concatWithCurrentFeed(response.feed);
        this.isResponseFailed = false;
      })
      .catch((err) => {
        this.isResponseFailed = true;
      })
      .finally(() => {
        this.isFeedLoading = false;
      });
  }

  concatWithCurrentFeed(data): void {
    this.ngZone.run(() => {
      this.feed = this.feed.concat(data);
    });
  }

  onLoadMore(): void {
    this.currentPage += 1;
    this.getPublications();
  }

  onScroll(): void {
    this.onLoadMore();
  }

  resetFeed(): void {
    this.feed = [];
    this.currentPage = 1;
  }

  selectFilters({ selectedTabType, selectTabFilters }): void {
    this.selectTabFilters = selectTabFilters;
    this.selectedTabType = selectedTabType;
    this.reloadFeed();
  }
}
