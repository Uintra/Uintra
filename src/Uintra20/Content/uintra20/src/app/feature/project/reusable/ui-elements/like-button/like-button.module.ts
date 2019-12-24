import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LikeButtonComponent } from './like-button.component';

@NgModule({
  declarations: [LikeButtonComponent],
  imports: [
    CommonModule
  ],
  exports: [
    LikeButtonComponent
  ]
})
export class LikeButtonModule { }
