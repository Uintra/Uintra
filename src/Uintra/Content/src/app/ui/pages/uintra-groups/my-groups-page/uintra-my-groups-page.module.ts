import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AS_DYNAMIC_COMPONENT, UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { UintraMyGroupsPage } from './uintra-my-groups-page.component';
import { GroupsWrapperModule } from 'src/app/feature/specific/groups/groups-wrapper/groups-wrapper.module';
import { GroupsListModule } from 'src/app/feature/specific/groups/groups-list/groups-list.module';

@NgModule({
  declarations: [UintraMyGroupsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraMyGroupsPage}]),
    UbaselineCoreModule,
    GroupsWrapperModule,
    GroupsListModule,
  ],
  providers: [{ provide: AS_DYNAMIC_COMPONENT, useValue: UintraMyGroupsPage }],
  entryComponents: [UintraMyGroupsPage]
})
export class UintraMyGroupsPageModule {}
