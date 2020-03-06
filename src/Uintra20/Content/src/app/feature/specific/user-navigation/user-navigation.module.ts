import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserNavigationComponent } from './user-navigation.component';
import { UserAvatarModule } from '../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';



@NgModule({
  declarations: [ UserNavigationComponent],
  imports: [
    CommonModule,
    ClickOutsideModule,
    UserAvatarModule,
    RouterModule,
    UlinkModule,
  ],
  exports: [
    UserNavigationComponent
  ]
})
export class UserNavigationModule { }
