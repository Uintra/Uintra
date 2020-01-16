import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavNotificationsComponent } from './nav-notifications.component';
import { HttpClientModule } from '@angular/common/http';
import { UserAvatarModule } from '../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { ClickOutsideDirective } from './helpers/click-outside.directive';
import { NotificationsItemComponent } from './notifications-item/notifications-item.component';

@NgModule({
  declarations: [NavNotificationsComponent, ClickOutsideDirective, NotificationsItemComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    UserAvatarModule,
    RouterModule
  ],
  exports: [ NavNotificationsComponent, NotificationsItemComponent ]
})
export class NavNotificationsModule { }
