import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { LatestActivitiesPanel } from './latest-activities-panel.component';

@NgModule({
  declarations: [
    LatestActivitiesPanel
  ],
  imports: [
    CommonModule
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: LatestActivitiesPanel }
  ],
  entryComponents: [
    LatestActivitiesPanel
  ]
})
export class LatestActivitiesPanelModule { }
