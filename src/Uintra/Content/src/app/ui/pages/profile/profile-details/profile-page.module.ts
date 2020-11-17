import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { ProfilePage } from './profile-page.component';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { TranslateModule } from '@ngx-translate/core';
import { AuthenticatedLayoutModule } from 'src/app/ui/main-layout/authenticated-layout/authenticated-layout.module';

@NgModule({
  declarations: [ProfilePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ProfilePage }]),
    UbaselineCoreModule,
    UserAvatarModule,
    UlinkModule,
    TranslateModule,
    AuthenticatedLayoutModule
  ],
  entryComponents: [ProfilePage]
})
export class ProfilePageModule { }

