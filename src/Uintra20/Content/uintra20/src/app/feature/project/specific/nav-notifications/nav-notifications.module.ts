import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavNotificationsComponent } from './nav-notifications.component';
import { HttpClientModule } from '@angular/common/http';
import { UserAvatarModule } from '../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { ClickOutsideDirective } from './helpers/click-outside.directive';

@NgModule({
  declarations: [NavNotificationsComponent, ClickOutsideDirective],
  imports: [
    CommonModule,
    HttpClientModule,
    UserAvatarModule,
    RouterModule
  ],
  exports: [ NavNotificationsComponent ]
})
export class NavNotificationsModule { }
