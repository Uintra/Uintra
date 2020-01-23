import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavNotificationsComponent } from './nav-notifications.component';
import { HttpClientModule } from '@angular/common/http';
import { UserAvatarModule } from '../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { NotificationsItemComponent } from './notifications-item/notifications-item.component';
import { ClickOutsideModule } from '../../reusable/directives/click-outside/click-outside.module';

@NgModule({
  declarations: [NavNotificationsComponent, NotificationsItemComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    UserAvatarModule,
    RouterModule,
    ClickOutsideModule
  ],
  exports: [ NavNotificationsComponent, NotificationsItemComponent ]
})
export class NavNotificationsModule { }
