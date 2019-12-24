import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'latest-activity',
  templateUrl: './latest-activity-item.component.html',
  styleUrls: ['./latest-activity-item.component.less']
})
export class LatestActivityComponent implements OnInit {
  @Input() activityType: string;
  @Input() activityDate: Date;
  @Input() activityId: string;
  @Input() activityDescription: string;

  constructor() { }

  ngOnInit() { }
}
