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

const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
  // TODO: Change this to your upload POST address:
  url: '/umbraco/api/file/UploadSingle',
  maxFiles: 10,
  maxFilesize: 50,
  addRemoveLinks: true,
  createImageThumbnails: true
};

@NgModule({
  declarations: [ActivityCreatePanel],
  imports: [
    CommonModule,
    NotImplementedModule,
    PublicationHeaderModule,
    UserAvatarModule,
    TagMultiselectModule,
    FormsModule,
    DropzoneModule,
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
