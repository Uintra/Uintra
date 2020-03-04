import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';

import { ActivityCreatePanel } from './activity-create-panel.component';

import { DropzoneModule, DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { DEFAULT_DROPZONE_CONFIG } from 'src/app/shared/constants/dropzone/drop-zone.const';
import { SocialCreateComponent } from '../../../feature/specific/activity/create/social-create/social-create.component';
import { NewsCreateComponent } from './sections/news-create/news-create.component';
import { QuillModule } from 'ngx-quill';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { RouterModule } from '@angular/router';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { TextInputModule } from 'src/app/feature/reusable/inputs/fields/text-input/text-input.module';
import { CheckboxInputModule } from 'src/app/feature/reusable/inputs/checkbox-input/checkbox-input.module';
import { SelectModule } from 'src/app/feature/reusable/inputs/select/select.module';
import { DatepickerFromToModule } from 'src/app/feature/specific/activity/datepicker-from-to/datepicker-from-to.module';
import { LocationPickerModule } from 'src/app/feature/reusable/ui-elements/location-picker/location-picker.module';
import { NewsFormModule } from 'src/app/feature/specific/activity/news-form/news-form.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { PublicationHeaderModule } from 'src/app/feature/reusable/ui-elements/publication-header/publication-header.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { TagMultiselectModule } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { RichTextEditorModule } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/shared/services/general/translations-loader';
import { SocialCreateModule } from '../../../feature/specific/activity/create/social-create/social-create.module';
import { NewsCreateModule } from './sections/news-create/news-create.module';

// TODO: remove unusable modules
@NgModule({
  declarations: [
    ActivityCreatePanel
  ],
  imports: [
    CommonModule,
    RouterModule,
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
    SqDatetimepickerModule,
    RichTextEditorModule.configure({
      modules: {
        counter: {
          maxLength: MAX_LENGTH
        }
      }
    }),
    LocationPickerModule,
    NewsFormModule,
    UlinkModule,
    GroupDetailsWrapperModule,
    TranslateModule,
    SocialCreateModule,
    NewsCreateModule,
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
export class ActivityCreatePanelModule { }
