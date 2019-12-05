import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserAvatarComponent } from './user-avatar/user-avatar.component';
import { PublicationHeaderComponent } from './publication-header/publication-header.component';

@NgModule({
  declarations: [
    UserAvatarComponent,
    PublicationHeaderComponent
  ],
  imports: [CommonModule],
  exports: [
    UserAvatarComponent,
    PublicationHeaderComponent
  ]
})
export class BlocksModule {}
