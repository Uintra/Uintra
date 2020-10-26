import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LikeButtonComponent } from './like-button.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [LikeButtonComponent],
  imports: [
    CommonModule,
    TranslateModule,
  ],
  exports: [
    LikeButtonComponent
  ]
})
export class LikeButtonModule { }
