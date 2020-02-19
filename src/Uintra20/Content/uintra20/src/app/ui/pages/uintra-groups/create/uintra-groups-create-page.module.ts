import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsCreatePage } from './uintra-groups-create-page.component';
import { GroupsWrapperModule } from 'src/app/feature/project/specific/groups/groups-wrapper/groups-wrapper.module';
import { GroupsFormModule } from 'src/app/feature/project/specific/groups/groups-form/groups-form.module';

@NgModule({
  declarations: [UintraGroupsCreatePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraGroupsCreatePage}]),
    UbaselineCoreModule,
    GroupsWrapperModule,
    GroupsFormModule,
  ],
  entryComponents: [UintraGroupsCreatePage]
})
export class UintraGroupsCreatePageModule {}
