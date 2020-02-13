import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsEditPage } from './uintra-groups-edit-page.component';
import { GroupsFormModule } from 'src/app/feature/project/specific/groups/groups-form/groups-form.module';

@NgModule({
  declarations: [UintraGroupsEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraGroupsEditPage}]),
    UbaselineCoreModule,
    GroupsFormModule,
  ],
  entryComponents: [UintraGroupsEditPage]
})
export class UintraGroupsEditPageModule {}
