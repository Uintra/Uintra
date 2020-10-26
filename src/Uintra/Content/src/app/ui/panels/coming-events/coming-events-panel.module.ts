import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { ComingEventsPanel } from './coming-events-panel.component';

@NgModule({
  declarations: [ComingEventsPanel],
  imports: [
    CommonModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: ComingEventsPanel}],
  entryComponents: [ComingEventsPanel]
})
export class ComingEventsPanelModule {}
