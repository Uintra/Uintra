import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropzoneWrapperComponent } from './dropzone-wrapper.component';
import { DropzoneModule, DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { DEFAULT_DROPZONE_CONFIG } from 'src/app/shared/constants/dropzone/drop-zone.const';



@NgModule({
  declarations: [DropzoneWrapperComponent],
  imports: [
    CommonModule,
    DropzoneModule
  ],
  providers: [
    {
      provide: DROPZONE_CONFIG,
      useValue: DEFAULT_DROPZONE_CONFIG
    }
  ],
  exports: [DropzoneWrapperComponent]
})
export class DropzoneWrapperModule { }
