import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NewsFormComponent } from "./news-form.component";
import { DatepickerFromToModule } from "../datepicker-from-to/datepicker-from-to.module";
import { SelectModule } from "../../../reusable/inputs/select/select.module";
import { TextInputModule } from "../../../reusable/inputs/fields/text-input/text-input.module";
import { RichTextEditorModule } from "../../../reusable/inputs/rich-text-editor/rich-text-editor.module";
import { TagMultiselectModule } from "../../../reusable/inputs/tag-multiselect/tag-multiselect.module";
import { LocationPickerModule } from "../../../reusable/ui-elements/location-picker/location-picker.module";
import { PinActivityModule } from "../pin-activity/pin-activity.module";
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { DropzoneWrapperModule } from '../../../reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module';
import { DropzoneExistingImagesModule } from '../../../reusable/ui-elements/dropzone-existing-images/dropzone-existing-images.module';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [NewsFormComponent],
  imports: [
    CommonModule,
    TagMultiselectModule,
    FormsModule,
    DropzoneWrapperModule,
    TextInputModule,
    QuillModule,
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
    PinActivityModule,
    DropzoneExistingImagesModule,
    TranslateModule,
  ],
  exports: [NewsFormComponent]
})
export class NewsFormModule {}
