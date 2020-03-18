import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { EventCreatePage } from './event-create-page.component';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { EventFormModule } from '../../../../feature/specific/activity/event-form/event-form.module';

@NgModule({
  declarations: [EventCreatePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: EventCreatePage}]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    EventFormModule,
  ],
  entryComponents: [EventCreatePage]
})
export class EventCreatePageModule {}
