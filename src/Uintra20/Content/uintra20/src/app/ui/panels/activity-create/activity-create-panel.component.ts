import { Component, ViewEncapsulation } from '@angular/core';
import { IActivityCreatePanel } from './activity-create-panel.interface';

@Component({
  selector: 'activity-create-panel',
  templateUrl: './activity-create-panel.html',
  styleUrls: ['./activity-create-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class ActivityCreatePanel {
  data: IActivityCreatePanel;
}