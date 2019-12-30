import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavNotificationsComponent } from './nav-notifications.component';
import { HttpClientModule } from '@angular/common/http';
import { UserAvatarModule } from '../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [NavNotificationsComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    UserAvatarModule,
    RouterModule
  ],
  exports: [ NavNotificationsComponent ]
})
export class NavNotificationsModule { }
