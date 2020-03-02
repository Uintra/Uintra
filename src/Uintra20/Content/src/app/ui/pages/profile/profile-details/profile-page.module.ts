import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { ProfilePage } from './profile-page.component';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';

@NgModule({
  declarations: [ProfilePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ProfilePage }]),
    UbaselineCoreModule,
    UserAvatarModule,
    UlinkModule
  ],
  entryComponents: [ProfilePage]
})
export class ProfilePageModule { }

