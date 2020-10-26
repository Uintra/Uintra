import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropzoneExistingImagesComponent } from './dropzone-existing-images.component';



@NgModule({
  declarations: [DropzoneExistingImagesComponent],
  imports: [
    CommonModule
  ],
  exports: [DropzoneExistingImagesComponent]
})
export class DropzoneExistingImagesModule { }
