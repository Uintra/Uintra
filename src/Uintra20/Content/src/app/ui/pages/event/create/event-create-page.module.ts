import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { EventCreatePage } from './event-create-page.component';

@NgModule({
  declarations: [EventCreatePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: EventCreatePage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [EventCreatePage]
})
export class EventCreatePageModule {}