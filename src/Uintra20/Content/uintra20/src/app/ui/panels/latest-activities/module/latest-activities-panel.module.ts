import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { LatestActivitiesPanelComponent } from '../component/latest-activities-panel.component';
import { LatestActivityComponent } from '../latest-activity/component/latest-activity.component';

@NgModule({
  declarations: [
    LatestActivitiesPanelComponent,
    LatestActivityComponent
  ],
  imports: [
    CommonModule,
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: LatestActivitiesPanelComponent }
  ],
  entryComponents: [
    LatestActivitiesPanelComponent
  ]
})
export class LatestActivitiesPanelModule { }
