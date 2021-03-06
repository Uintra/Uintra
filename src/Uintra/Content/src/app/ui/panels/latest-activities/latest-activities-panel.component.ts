import { Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import { PublicationsService, IFeedListRequest } from '../central-feed/helpers/publications.service';
import { SignalrService } from 'src/app/shared/services/general/signalr.service';
import { CentralFeedFiltersService } from '../central-feed/central-feed-filters/central-feed-filters.service';
import { ILatestActivitiesPanel } from 'src/app/shared/interfaces/panels/latest-activities/latest-activities-panel.interface';
import { Subscription } from 'rxjs';
import { IPublicationsResponse } from 'src/app/shared/interfaces/panels/central-feed/central-feed-panel.interface';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Indexer } from '../../../shared/abstractions/indexer';

@Component({
  selector: 'latest-activities-panel',
  templateUrl: './latest-activities-panel.component.html',
  styleUrls: ['./latest-activities-panel.component.less']
})
export class LatestActivitiesPanelComponent extends Indexer<number> implements OnInit, OnDestroy {

  constructor(
    private publicationsService: PublicationsService,
    private signalrService: SignalrService,
    private ngZone: NgZone,
    private CFFilterService: CentralFeedFiltersService,
    private sanitizer: DomSanitizer,
  ) {
    super();
  }

  private $publications: Subscription;
  public data: ILatestActivitiesPanel;
  public teaser: SafeHtml;

  public ngOnInit(): void {
    this.signalrService.getReloadFeedSubjects().subscribe(() => this.reload());
    this.teaser = this.sanitizer.bypassSecurityTrustHtml(this.data.teaser);
  }

  public ngOnDestroy(): void {
    if (this.$publications) { this.$publications.unsubscribe(); }
  }

  private reload(): void {
    this.cleanLatestActivity();

    this.$publications = this.publicationsService.getPublications(this.requestModel)
      .subscribe(
        (next: IPublicationsResponse) => {
          this.ngZone.run(() => this.data.feed = next.feed.slice(0, 5));
        }
      );
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
}


