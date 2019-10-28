import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LinkComponent } from './link.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [LinkComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [LinkComponent]
})
export class LinkModule { }
