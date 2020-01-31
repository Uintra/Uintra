import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NewsFormComponent } from "./news-form.component";
import { DatepickerFromToModule } from "../../../reusable/inputs/datepicker-from-to/datepicker-from-to.module";
import { SelectModule } from "../../../reusable/inputs/select/select.module";
import { TextInputModule } from "../../../reusable/inputs/fields/text-input/text-input.module";
import { RichTextEditorModule } from "../../../reusable/inputs/rich-text-editor/rich-text-editor.module";
import { MAX_LENGTH } from "src/app/constants/activity/create/activity-create-const";
import { TagMultiselectModule } from "../../../reusable/inputs/tag-multiselect/tag-multiselect.module";
import { LocationPickerModule } from "../../../reusable/ui-elements/location-picker/location-picker.module";
import { DropzoneModule } from "ngx-dropzone-wrapper";
import { PinActivityModule } from "../pin-activity/pin-activity.module";
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

@NgModule({
  declarations: [NewsFormComponent],
  imports: [
    CommonModule,
    TagMultiselectModule,
    FormsModule,
    DropzoneModule,
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
    PinActivityModule
  ],
  exports: [NewsFormComponent]
})
export class NewsFormModule {}
