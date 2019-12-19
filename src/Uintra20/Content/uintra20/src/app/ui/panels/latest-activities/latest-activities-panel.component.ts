import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ILatestActivitiesPanel } from './latest-activities-panel.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'latest-activities-panel',
  templateUrl: './latest-activities-panel.html',
  styleUrls: ['./latest-activities-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LatestActivitiesPanel implements OnInit {

  public data: ILatestActivitiesPanel;

  constructor(
    private router: Router
  ) { }

  public ngOnInit = (): void => {
    debugger;
  }

  public seeAll = (): void => {
    console.log('navigated to bulletins');
    // this.router.navigate(['bulletins']);
  }
}
