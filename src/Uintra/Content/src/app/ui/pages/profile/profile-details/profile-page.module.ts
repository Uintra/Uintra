import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AS_DYNAMIC_COMPONENT, UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { ProfilePage } from './profile-page.component';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [ProfilePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ProfilePage }]),
    UbaselineCoreModule,
    UserAvatarModule,
    UlinkModule,
    TranslateModule,
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: ProfilePage }
  ],
  entryComponents: [ProfilePage]
})
export class ProfilePageModule { }

