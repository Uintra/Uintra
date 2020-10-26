import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListComponent } from './user-list.component';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { UserAvatarModule } from '../../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { NotificationsItemComponent } from '../../nav-notifications/notifications-item/notifications-item.component';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';



@NgModule({
  declarations: [UserListComponent],
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    UserAvatarModule,
    RouterModule,
    UlinkModule,
    ClickOutsideModule,
  ],
  providers: [ModalService],
  exports: [UserListComponent],
  entryComponents: [UserListComponent]
})
export class UserListModule { }
