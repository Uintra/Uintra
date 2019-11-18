import { Component, OnInit, Input } from '@angular/core';

interface CreateActivityPanelData {
  activityType: string,
  tags: any[]
}

@Component({
  selector: 'app-create-activity-panel',
  templateUrl: './create-activity-panel.component.html',
  styleUrls: ['./create-activity-panel.component.less']
})
export class CreateActivityPanelComponent implements OnInit {
  @Input() data: CreateActivityPanelData;
  constructor() { }

  ngOnInit() {

  }
}
