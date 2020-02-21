import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsMembersPage } from './uintra-groups-members-page.component';
import { GroupDetailsWrapperModule } from 'src/app/feature/project/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { TextInputModule } from 'src/app/feature/project/reusable/inputs/fields/text-input/text-input.module';
import { UserAvatarModule } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module';

@NgModule({
  declarations: [UintraGroupsMembersPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraGroupsMembersPage}]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    TextInputModule,
    UserAvatarModule
  ],
  entryComponents: [UintraGroupsMembersPage]
})
export class UintraGroupsMembersPageModule {}
