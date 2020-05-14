import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { UintraGroupsRoomPage } from './uintra-groups-room-page.component';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { SocialCreateModule } from 'src/app/feature/specific/activity/create/social-create/social-create.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [UintraGroupsRoomPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraGroupsRoomPage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    SocialCreateModule,
    UlinkModule,
    TranslateModule,
  ],
  entryComponents: [UintraGroupsRoomPage]
})
export class UintraGroupsRoomPageModule {}
