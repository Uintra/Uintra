import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublicationHeaderComponent } from './publication-header.component';
import { UserAvatarModule } from '../user-avatar/user-avatar.module';


@NgModule({
  declarations: [PublicationHeaderComponent],
  imports: [
    CommonModule,
    UserAvatarModule
  ],
  exports: [
    PublicationHeaderComponent
  ]
})
export class PublicationHeaderModule { }
