import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateActivityBulletinsComponent } from './create-activity-bulletins.component';
import { TagMultiselectComponent } from './tag-multiselect/tag-multiselect.component';
import { BulletinsTextEditorComponent } from './bulletins-text-editor/bulletins-text-editor.component';
import { TagItemComponent } from './tag-multiselect/tag-item/tag-item.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { DropzoneModule, DROPZONE_CONFIG, DropzoneConfigInterface } from 'ngx-dropzone-wrapper';
import { RichTextEditorModule } from 'src/app/shared/components/fields/rich-text-editor/rich-text-editor.module';

const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
  // TODO: Change this to your upload POST address:
  url: "https://httpbin.org/post",
  maxFilesize: 50,
  acceptedFiles: "image/*",
  addRemoveLinks: true
};

@NgModule({
  declarations: [
    CreateActivityBulletinsComponent,
    TagMultiselectComponent,
    BulletinsTextEditorComponent,
    TagItemComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule,
    DropzoneModule,
    RichTextEditorModule
  ],
  providers: [
    {
      provide: DROPZONE_CONFIG,
      useValue: DEFAULT_DROPZONE_CONFIG
    }
  ],
  exports: [
    CreateActivityBulletinsComponent
  ]
})
export class CreateActivityBulletinsModule {}
