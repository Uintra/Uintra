import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LinkPreviewComponent } from './link-preview.component';



@NgModule({
  declarations: [LinkPreviewComponent],
  imports: [
    CommonModule
  ],
  exports: [LinkPreviewComponent]
})
export class LinkPreviewModule { }
