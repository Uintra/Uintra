import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublicationHeaderComponent } from './publication-header.component';
import { UserAvatarModule } from '../user-avatar/user-avatar.module';
import { RouterModule } from '@angular/router';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';


@NgModule({
  declarations: [PublicationHeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    UlinkModule,
    UserAvatarModule
  ],
  exports: [
    PublicationHeaderComponent
  ]
})
export class PublicationHeaderModule { }
