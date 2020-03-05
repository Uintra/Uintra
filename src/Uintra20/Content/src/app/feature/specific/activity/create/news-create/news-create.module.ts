import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsCreateComponent } from './news-create.component';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { TagMultiselectModule } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { FormsModule } from '@angular/forms';
import { DropzoneModule } from 'ngx-dropzone-wrapper';
import { TextInputModule } from 'src/app/feature/reusable/inputs/fields/text-input/text-input.module';
import { QuillModule } from 'ngx-quill';
import { CheckboxInputModule } from 'src/app/feature/reusable/inputs/checkbox-input/checkbox-input.module';
import { SelectModule } from 'src/app/feature/reusable/inputs/select/select.module';
import { DatepickerFromToModule } from 'src/app/feature/specific/activity/datepicker-from-to/datepicker-from-to.module';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { RichTextEditorModule } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { LocationPickerModule } from 'src/app/feature/reusable/ui-elements/location-picker/location-picker.module';
import { NewsFormModule } from 'src/app/feature/specific/activity/news-form/news-form.module';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';



@NgModule({
  declarations: [NewsCreateComponent],
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
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
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: ActivityCreatePanel },
    {
      provide: DROPZONE_CONFIG,
      useValue: DEFAULT_DROPZONE_CONFIG
    }
  ],
  exports: [NewsCreateComponent]
})
export class NewsCreateModule { }
