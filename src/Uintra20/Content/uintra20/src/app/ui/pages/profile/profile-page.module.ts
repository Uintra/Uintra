import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { ProfilePage } from './profile-page.component';
import { UserAvatarModule } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module';

@NgModule({
  declarations: [ProfilePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ProfilePage }]),
    UbaselineCoreModule,
    UserAvatarModule
  ],
  entryComponents: [ProfilePage]
})
export class ProfilePageModule { }

