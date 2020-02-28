import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsRoomPage } from './uintra-groups-room-page.component';
import { GroupDetailsWrapperModule } from 'src/app/feature/project/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { ActivityCreatePanelModule } from 'src/app/ui/panels/activity-create/activity-create-panel.module';
import { UlinkModule } from 'src/app/services/pipes/link/ulink.module';
import { CanDeactivateGuard } from 'src/app/services/general/can-deactivate.service';

@NgModule({
  declarations: [UintraGroupsRoomPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraGroupsRoomPage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    ActivityCreatePanelModule,
    UlinkModule,
  ],
  entryComponents: [UintraGroupsRoomPage]
})
export class UintraGroupsRoomPageModule {}
