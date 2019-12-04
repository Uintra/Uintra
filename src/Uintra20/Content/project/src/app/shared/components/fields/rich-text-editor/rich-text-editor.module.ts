import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RichTextEditorComponent } from './rich-text-editor.component';
import { QuillModule } from 'ngx-quill';
import { DropzoneModule, DropzoneConfigInterface, DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
  // TODO: Change this to your upload POST address:
  url: '',
  maxFilesize: 50,
  acceptedFiles: '',
  addRemoveLinks: true
};

@NgModule({
  declarations: [RichTextEditorComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule,
    DropzoneModule
  ],
  providers: [
    {
      provide: DROPZONE_CONFIG,
      useValue: DEFAULT_DROPZONE_CONFIG
    }
  ],
  exports: [
    RichTextEditorComponent
  ]
})
export class RichTextEditorModule { }
