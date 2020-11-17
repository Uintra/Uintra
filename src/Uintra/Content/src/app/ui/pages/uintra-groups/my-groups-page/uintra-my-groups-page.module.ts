import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { UintraMyGroupsPage } from './uintra-my-groups-page.component';
import { GroupsWrapperModule } from 'src/app/feature/specific/groups/groups-wrapper/groups-wrapper.module';
import { GroupsListModule } from 'src/app/feature/specific/groups/groups-list/groups-list.module';
import { AuthenticatedLayoutModule } from 'src/app/ui/main-layout/authenticated-layout/authenticated-layout.module';

@NgModule({
  declarations: [UintraMyGroupsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraMyGroupsPage}]),
    UbaselineCoreModule,
    GroupsWrapperModule,
    GroupsListModule,
    AuthenticatedLayoutModule
  ],
  entryComponents: [UintraMyGroupsPage]
})
export class UintraMyGroupsPageModule {}
