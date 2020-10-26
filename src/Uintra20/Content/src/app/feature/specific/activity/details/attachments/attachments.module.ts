import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AttachmentsComponent } from './attachments.component';



@NgModule({
  declarations: [AttachmentsComponent],
  imports: [
    CommonModule
  ],
  exports: [AttachmentsComponent]
})
export class AttachmentsModule { }
