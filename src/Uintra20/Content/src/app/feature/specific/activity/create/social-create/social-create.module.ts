import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { TranslateModule } from '@ngx-translate/core';
import { QuillModule } from 'ngx-quill';
import { FormsModule } from '@angular/forms';
import { DropzoneModule, DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { RouterModule } from '@angular/router';
import { PublicationHeaderModule } from 'src/app/feature/reusable/ui-elements/publication-header/publication-header.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { TagMultiselectModule } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { RichTextEditorModule } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { DEFAULT_DROPZONE_CONFIG } from 'src/app/shared/constants/dropzone/drop-zone.const';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { SocialCreateComponent } from './social-create.component';



@NgModule({
  declarations: [SocialCreateComponent],
  imports: [
    CommonModule,
    UlinkModule,
    GroupDetailsWrapperModule,
    TranslateModule,
    QuillModule,
    FormsModule,
    DropzoneModule,
    RouterModule,
    PublicationHeaderModule,
    UserAvatarModule,
    TagMultiselectModule,
    RichTextEditorModule.configure({
      modules: {
        counter: {
          maxLength: MAX_LENGTH
        }
      }
    }),
  ],
  providers: [
    {
      provide: DROPZONE_CONFIG,
      useValue: DEFAULT_DROPZONE_CONFIG
    }
  ],
  exports: [SocialCreateComponent]
})
export class SocialCreateModule { }
