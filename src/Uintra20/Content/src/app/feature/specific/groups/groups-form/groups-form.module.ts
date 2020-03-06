import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsFormComponent } from './groups-form.component';
import { TextInputModule } from '../../../reusable/inputs/fields/text-input/text-input.module';
import { RichTextEditorModule } from '../../../reusable/inputs/rich-text-editor/rich-text-editor.module';
import { FormsModule } from '@angular/forms';
import { DropzoneWrapperModule } from '../../../reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module';
import { DropzoneExistingImagesModule } from '../../../reusable/ui-elements/dropzone-existing-images/dropzone-existing-images.module';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [GroupsFormComponent],
  imports: [
    CommonModule,
    TextInputModule,
    RichTextEditorModule,
    FormsModule,
    DropzoneWrapperModule,
    DropzoneExistingImagesModule,
    TranslateModule,
  ],
  exports: [GroupsFormComponent]
})
export class GroupsFormModule { }
