import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AS_DYNAMIC_COMPONENT, UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { UintraGroupsMembersPage } from './uintra-groups-members-page.component';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { TextInputModule } from 'src/app/feature/reusable/inputs/fields/text-input/text-input.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';

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
  providers: [{ provide: AS_DYNAMIC_COMPONENT, useValue: UintraGroupsMembersPage }],
  entryComponents: [UintraGroupsMembersPage]
})
export class UintraGroupsMembersPageModule {}
