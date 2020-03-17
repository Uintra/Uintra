import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { EventEditPage } from './event-edit-page.component';

@NgModule({
  declarations: [EventEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: EventEditPage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [EventEditPage]
})
export class EventEditPageModule {}