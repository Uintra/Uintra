import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule, AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { RichTextEditorModule } from 'src/app/feature/project/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { TagMultiselectModule } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { FormsModule } from '@angular/forms';
import { DropzoneModule, DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { SocialEditPageComponent } from './social-edit-page.component';
import { HttpClientModule } from '@angular/common/http';
import { MAX_LENGTH } from 'src/app/constants/activity/create/activity-create-const';
import { DEFAULT_DROPZONE_CONFIG } from 'src/app/constants/dropzone/drop-zone.const';

@NgModule({
  declarations:
    [
      SocialEditPageComponent
    ],
  imports:
    [
      CommonModule,
      RouterModule.forChild(
        [
          { path: '', component: SocialEditPageComponent }
        ]),
      UbaselineCoreModule,
      TagMultiselectModule,
      FormsModule,
      DropzoneModule,
      HttpClientModule,
      RichTextEditorModule.configure({
        modules: {
          counter: {
            maxLength: MAX_LENGTH
          }
        }
      })
    ],
  providers:
    [
      { provide: AS_DYNAMIC_COMPONENT, useValue: SocialEditPageComponent },
      {
        provide: DROPZONE_CONFIG,
        useValue: DEFAULT_DROPZONE_CONFIG
      }
    ],
  entryComponents:
    [
      SocialEditPageComponent
    ]
})
export class SocialEditPageModule { }

