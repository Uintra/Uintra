import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { IActivityCreatePanel } from './activity-create-panel.interface';
import { CreateActivityService } from 'src/app/services/createActivity/create-activity.service';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { ModalService } from 'src/app/services/general/modal.service';
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

  constructor(private socialContentService: CreateActivityService, private modalService: ModalService) { }

  ngOnInit() {
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.activityType = parsed.activityType;
  }
}
