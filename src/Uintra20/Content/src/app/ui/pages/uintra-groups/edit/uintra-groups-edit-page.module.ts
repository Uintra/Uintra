import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsEditPage } from './uintra-groups-edit-page.component';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { GroupsFormModule } from 'src/app/feature/specific/groups/groups-form/groups-form.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';

@NgModule({
  declarations: [UintraGroupsEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraGroupsEditPage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    GroupsFormModule,
    GroupDetailsWrapperModule,
  ],
  entryComponents: [UintraGroupsEditPage]
})
export class UintraGroupsEditPageModule {}
