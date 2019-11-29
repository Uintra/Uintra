import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { ActivityCreatePanel } from './activity-create-Panel.component';

@NgModule({
  declarations: [ActivityCreatePanel],
  imports: [
    CommonModule,
    NotImplementedModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: ActivityCreatePanel}],
  entryComponents: [ActivityCreatePanel]
})
export class ActivityCreatePanelModule {}