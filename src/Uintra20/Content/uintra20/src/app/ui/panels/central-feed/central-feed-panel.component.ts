import { Component, ViewEncapsulation, OnInit, NgZone } from "@angular/core";
import { ICentralFeedPanel } from "./central-feed-panel.interface";
import { UmbracoFlatPropertyModel, IUmbracoProperty } from "@ubaseline/next";
import { PublicationsService } from "./helpers/publications.service";
import { ActivityService } from "src/app/feature/project/specific/activity/activity.service";
import { SignalrService } from "src/app/services/general/signalr.service";

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
  selector: "central-feed-panel",
  templateUrl: "./central-feed-panel.html",
  styleUrls: ["./central-feed-panel.less"],
  encapsulation: ViewEncapsulation.None
})
export class CentralFeedPanel implements OnInit {
  data: ICentralFeedPanel;
  tabs: Array<any> = null;
  // TODO: replace 'any' after server side will be done
  selectTabFilters: Array<any>;
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
    private ngZone: NgZone
  ) {}

  ngOnInit() {
    this.tabs = this.filtersBuilder();

    // this.socialService.feedRefreshTrigger$.subscribe(() => {
    //   this.reloadFeed();
    // });

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
      title: "All",
      filters: [
        {
          key: "ShowPinned",
          title: "Show only Important",
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
      FilterState[filter.key] = filter.isActive;
    });
    const data = {
      TypeId: this.selectedTabType,
      FilterState,
      Page: this.currentPage,
      groupId: this.data.groupId.data.value
    };

    this.isFeedLoading = true;
    // TODO: replace 'any' after server side will be done
    this.publicationsService
      .getPublications(data)
      .then((response: any) => {
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
