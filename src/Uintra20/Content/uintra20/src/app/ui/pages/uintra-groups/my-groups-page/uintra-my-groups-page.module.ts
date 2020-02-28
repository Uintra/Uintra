import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraMyGroupsPage } from './uintra-my-groups-page.component';
import { GroupsListModule } from 'src/app/feature/project/specific/groups/groups-list/groups-list.module';
import { GroupsWrapperModule } from 'src/app/feature/project/specific/groups/groups-wrapper/groups-wrapper.module';

@NgModule({
  declarations: [UintraMyGroupsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraMyGroupsPage}]),
    UbaselineCoreModule,
    GroupsWrapperModule,
    GroupsListModule,
  ],
  entryComponents: [UintraMyGroupsPage]
})
export class UintraMyGroupsPageModule {}
