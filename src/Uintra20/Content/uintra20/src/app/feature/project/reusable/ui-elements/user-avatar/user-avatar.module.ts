import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserAvatarComponent } from './user-avatar.component';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [UserAvatarComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    UserAvatarComponent
  ]
})
export class UserAvatarModule { }
