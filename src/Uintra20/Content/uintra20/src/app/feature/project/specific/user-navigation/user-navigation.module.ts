import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserNavigationComponent } from './user-navigation.component';
import { ClickOutsideModule } from '../../reusable/directives/click-outside/click-outside.module';
import { UserAvatarModule } from '../../reusable/ui-elements/user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { UlinkModule } from 'src/app/services/pipes/link/ulink.module';



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
