import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { LatestActivitiesPanelComponent } from './latest-activities-panel.component';
import { LatestActivityComponent } from './latest-activity-item/latest-activity-item.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    LatestActivitiesPanelComponent,
    LatestActivityComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: LatestActivitiesPanelComponent }
  ],
  entryComponents: [
    LatestActivitiesPanelComponent
  ]
})
export class LatestActivitiesPanelModule { }
