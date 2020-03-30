import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { EventEditPage } from './event-edit-page.component';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { EventFormModule } from '../../../../feature/specific/activity/event-form/event-form.module';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';

@NgModule({
  declarations: [EventEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: EventEditPage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    EventFormModule,
  ],
  entryComponents: [EventEditPage]
})
export class EventEditPageModule {}
