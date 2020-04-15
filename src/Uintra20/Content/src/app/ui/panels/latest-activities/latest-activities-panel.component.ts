import { Component, OnInit, ViewEncapsulation, NgZone } from '@angular/core';
import { ILatestActivitiesPanel } from './latest-activities-panel.interface';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { PublicationsService, IFeedListRequest } from '../central-feed/helpers/publications.service';
import { SignalrService } from 'src/app/shared/services/general/signalr.service';
import { IPublicationsResponse, IPublication } from '../central-feed/central-feed-panel.interface';
import { CentralFeedFiltersService } from '../central-feed/central-feed-filters/central-feed-filters.service';

@Component({
  selector: 'latest-activities-panel',
  templateUrl: './latest-activities-panel.component.html',
  styleUrls: ['./latest-activities-panel.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class LatestActivitiesPanelComponent implements OnInit {

  constructor(
    private publicationsService: PublicationsService,
    private signalrService: SignalrService,
    private ngZone: NgZone,
    private CFFilterService: CentralFeedFiltersService,
  ) { }
  //TODO: Change data interface from any to ILatestActivitiesPanel once you remove UFP from this panel and remove first three lines in ngOnInit()
  public data: any;
  // public data: ILatestActivitiesPanel;
  public title: string;
  public teaser: string;
  public activities: Array<IPublication> = new Array<IPublication>();
  public showAll: false;
  public activityType: number;

  public ngOnInit(): void {
    if (this.data.get) {
      this.data = this.data.get();
    }
    this.parse();
    this.signalrService.getReloadFeedSubjects().subscribe(() => this.reload());
  }

  private parse(): void {
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.title = parsed.title;
    this.teaser = parsed.teaser;
    this.activities = Object.values(parsed.feed);
    this.showAll = parsed.showSeeAllButton;
    this.activityType = parsed.activityType.activityId;
  }

  private reload(): void {
    this.cleanLatestActivity();

    this.publicationsService.getPublications(this.requestModel)
      .then((response: IPublicationsResponse) =>
        this.ngZone.run(() => this.activities = response.feed.slice(0, 5)));
  }

  private get requestModel(): IFeedListRequest {
    return {
      TypeId: 0,
      FilterState: {
        ShowPinned: false
      },
      Page: 1
    };
  }

  onSeeAllClick() {
    this.CFFilterService.changeFilter(this.activityType);
  }

  private cleanLatestActivity = () =>
    this.activities = []
}


