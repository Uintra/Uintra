import { Component, ViewEncapsulation, OnInit, NgZone, OnDestroy } from "@angular/core";
import { ICentralFeedPanel, IPublicationsResponse, IFilterState } from "./central-feed-panel.interface";
import { UmbracoFlatPropertyModel } from "@ubaseline/next";
import { PublicationsService } from "./helpers/publications.service";
import { SignalrService } from "src/app/shared/services/general/signalr.service";
import { TranslateService } from '@ngx-translate/core';
import { finalize } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: "central-feed-panel",
  templateUrl: "./central-feed-panel.html",
  styleUrls: ["./central-feed-panel.less"],
  encapsulation: ViewEncapsulation.None
})
export class CentralFeedPanel implements OnInit, OnDestroy {
  //TODO: Change data interface from any to ICentralFeedPanel once you remove UFP from this panel and remove first three lines in ngOnInit()
  data: any;
  // data: ICentralFeedPanel;
  tabs: Array<any> = null;
  selectTabFilters: Array<IFilterState>;
  selectedTabType: number;
  feed: Array<any> = [];
  currentPage = 1;
  isFeedLoading = false;
  isResponseFailed = false;
  isScrollDisabled = false;
  private $publications: Subscription;

  constructor(
    private publicationsService: PublicationsService,
    private signalrService: SignalrService,
    private ngZone: NgZone,
    private translate: TranslateService,
  ) { }

  public ngOnInit(): void {
    if (this.data.get) {
      this.data = this.data.get();
    }
    this.tabs = this.filtersBuilder();

    this.signalrService.getReloadFeedSubjects().subscribe(s => {
      this.reloadFeed();
    });
  }

  public ngOnDestroy(): void {
    if (this.$publications) { this.$publications.unsubscribe(); }
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

    this.$publications = this.publicationsService
      .getPublications(data)
      .pipe(finalize(() => this.isFeedLoading = false))
      .subscribe(
        (next: IPublicationsResponse) => {
          this.isScrollDisabled = next.feed.length === 0;
          this.concatWithCurrentFeed(next.feed);
          this.isResponseFailed = false;
        },
        (error: HttpErrorResponse) => this.isResponseFailed = true);

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
