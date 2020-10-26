import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsListComponent } from './groups-list.component';
import { UserAvatarModule } from '../../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';



@NgModule({
  declarations: [GroupsListComponent],
  imports: [
    CommonModule,
    UserAvatarModule,
    RouterModule,
    UlinkModule,
    InfiniteScrollModule,
  ],
  exports: [GroupsListComponent]
})
export class GroupsListModule { }
