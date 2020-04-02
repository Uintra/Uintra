import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ILatestActivitiesPanel } from './latest-activities-panel.interface';
import { FeedStoreService } from 'src/app/shared/services/general/feed-store.service';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'latest-activities-panel',
  templateUrl: './latest-activities-panel.component.html',
  styleUrls: ['./latest-activities-panel.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class LatestActivitiesPanelComponent implements OnInit {

  constructor(
    private feedStoreService: FeedStoreService
  ) { }

  public readonly data: ILatestActivitiesPanel;
  public title: string;
  public teaser: string;
  public activityCells: any;
  public showAll: false;

  public ngOnInit(): void {
    this.parse();
  }
  private parse(): void {
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.title = parsed.title;
    this.teaser = parsed.teaser;
    this.activityCells = Object.values(parsed.feed);
    this.showAll = parsed.showSeeAllButton;
  }
}
