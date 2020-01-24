import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { LatestActivitiesPanelComponent } from './latest-activities-panel.component';
import { LatestActivityComponent } from './latest-activity-item/latest-activity-item.component';
import { RouterModule } from '@angular/router';
import { UlinkModule } from 'src/app/services/pipes/link/ulink.module';

@NgModule({
  declarations: [
    LatestActivitiesPanelComponent,
    LatestActivityComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    UlinkModule
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: LatestActivitiesPanelComponent }
  ],
  entryComponents: [
    LatestActivitiesPanelComponent
  ]
})
export class LatestActivitiesPanelModule { }
