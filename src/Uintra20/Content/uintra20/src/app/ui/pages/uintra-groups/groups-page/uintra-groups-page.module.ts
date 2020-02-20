import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsPage } from './uintra-groups-page.component';
import { GroupsWrapperModule } from 'src/app/feature/project/specific/groups/groups-wrapper/groups-wrapper.module';
import { GroupsListModule } from 'src/app/feature/project/specific/groups/groups-list/groups-list.module';

@NgModule({
  declarations: [UintraGroupsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: UintraGroupsPage }]),
    UbaselineCoreModule,
    GroupsWrapperModule,
    GroupsListModule,
  ],
  entryComponents: [UintraGroupsPage]
})
export class UintraGroupsPageModule { }

