import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { LatestActivitiesPanelComponent } from '../component/latest-activities-panel.component';

@NgModule({
  declarations: [
    LatestActivitiesPanelComponent
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
