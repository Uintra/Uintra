import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'latest-activities-panel',
  templateUrl: './latest-activities-panel.html',
  styleUrls: ['./latest-activities-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LatestActivitiesPanel implements OnInit {

  public data: any;

  constructor() { }

  ngOnInit() {
    let data = this.data;
  }
}
