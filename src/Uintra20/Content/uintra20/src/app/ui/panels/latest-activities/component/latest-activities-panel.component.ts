import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { ILatestActivitiesPanel } from '../contract/latest-activities-panel.interface';

@Component({
  selector: 'latest-activities-panel',
  templateUrl: './latest-activities-panel.html',
  styleUrls: ['./latest-activities-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LatestActivitiesPanelComponent implements OnInit {

  constructor(
    private router: Router
  ) { }
  public readonly data: ILatestActivitiesPanel;
  public title: string;
  public activityCells: any;
  public showAll: false;

  public ngOnInit(): void {
    this.title = this.data.title.get();
    this.activityCells = Object.values(this.data.feed.get());
    this.showAll = this.data.showSeeAllButton.get();
  }

  public seeAll = (): void => {
    console.log('navigated to bulletins');
    // this.router.navigate(['bulletins']);
  }
}


