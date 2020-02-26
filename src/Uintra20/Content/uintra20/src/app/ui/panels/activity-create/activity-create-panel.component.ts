import { Component, ViewEncapsulation, OnInit, Output, EventEmitter } from '@angular/core';
import { IActivityCreatePanel } from './activity-create-panel.interface';
import { ActivityEnum } from 'src/app/feature/shared/enums/activity-type.enum';

@Component({
  selector: 'activity-create-panel',
  templateUrl: './activity-create-panel.component.html',
  styleUrls: ['./activity-create-panel.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class ActivityCreatePanel implements OnInit {
  data: IActivityCreatePanel;
  activityTypes = ActivityEnum;
  activityType: ActivityEnum;

  constructor() { }

  ngOnInit() {
    this.activityType = this.data.activityType.get();
  }
}
