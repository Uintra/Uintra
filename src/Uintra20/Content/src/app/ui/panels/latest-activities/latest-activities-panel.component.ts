import { Component, OnInit, ViewEncapsulation, NgZone } from '@angular/core';
import { PublicationsService, IFeedListRequest } from '../central-feed/helpers/publications.service';
import { SignalrService } from 'src/app/shared/services/general/signalr.service';
import { IPublicationsResponse } from '../central-feed/central-feed-panel.interface';
import { CentralFeedFiltersService } from '../central-feed/central-feed-filters/central-feed-filters.service';
import { ILatestActivitiesPanel } from 'src/app/shared/interfaces/panels/latest-activities/latest-activities-panel.interface';

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
  ) {
  }
  public data: ILatestActivitiesPanel;

  public ngOnInit(): void {
    this.signalrService.getReloadFeedSubjects().subscribe(() => this.reload());
  }

  private reload(): void {
    this.cleanLatestActivity();

    this.publicationsService.getPublications(this.requestModel)
      .then((response: IPublicationsResponse) =>
        this.ngZone.run(() => this.data.feed = response.feed.slice(0, 5)));
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

  public onSeeAllClick(): void {
    this.CFFilterService.changeFilter(this.data.activityType.activityId);
  }

  private cleanLatestActivity = () =>
    this.data.feed = []

  public index = (index): number => index;
}


