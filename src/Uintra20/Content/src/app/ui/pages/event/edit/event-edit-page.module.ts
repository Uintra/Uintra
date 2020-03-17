import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { EventEditPage } from './event-edit-page.component';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { EventCreateModule } from 'src/app/feature/specific/activity/create/event-create/event-create.module';

@NgModule({
  declarations: [EventEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: EventEditPage}]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    EventCreateModule,
  ],
  entryComponents: [EventEditPage]
})
export class EventEditPageModule {}
