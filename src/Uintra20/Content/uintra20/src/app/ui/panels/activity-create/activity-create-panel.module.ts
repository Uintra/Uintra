import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';

import { ActivityCreatePanel } from './activity-create-panel.component';
import { RichTextEditorModule } from 'src/app/feature/project/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { PublicationHeaderModule } from 'src/app/feature/project/reusable/ui-elements/publication-header/publication-header.module';
import { UserAvatarModule } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module';
import { TagMultiselectModule } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.module';

import { DropzoneModule, DropzoneConfigInterface, DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import 'quill-emoji/dist/quill-emoji';
import { MAX_LENGTH } from './_constants.js';
import { NewsCreateComponent } from './sections/news-create/news-create.component';
import { SocialCreateComponent } from './sections/social-create/social-create.component';
import { TextInputModule } from 'src/app/feature/project/reusable/inputs/fields/text-input/text-input.module';
import { QuillModule } from 'ngx-quill';
import { CheckboxInputModule } from 'src/app/feature/project/reusable/inputs/checkbox-input/checkbox-input.module';
import { SelectModule } from 'src/app/feature/project/reusable/inputs/select/select.module';
import { DatepickerFromToModule } from 'src/app/feature/project/reusable/inputs/datepicker-from-to/datepicker-from-to.module';

const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
  // TODO: Change this to your upload POST address:
  url: '/umbraco/api/file/UploadSingle',
  maxFiles: 10,
  maxFilesize: 50,
  addRemoveLinks: true,
  createImageThumbnails: true
};

@NgModule({
  declarations: [
    ActivityCreatePanel,
    SocialCreateComponent,
    NewsCreateComponent],
  imports: [
    CommonModule,
    NotImplementedModule,
    PublicationHeaderModule,
    UserAvatarModule,
    TagMultiselectModule,
    FormsModule,
    DropzoneModule,
    TextInputModule,
    QuillModule,
    CheckboxInputModule,
    SelectModule,
    DatepickerFromToModule,
    RichTextEditorModule.configure({
      modules: {
        'emoji-toolbar': true,
        counter: {
          maxLength: MAX_LENGTH
        }
      }
    })
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: ActivityCreatePanel },
    {
      provide: DROPZONE_CONFIG,
      useValue: DEFAULT_DROPZONE_CONFIG
    }
  ],
  entryComponents: [ActivityCreatePanel]
})
export class ActivityCreatePanelModule {}
