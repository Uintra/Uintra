import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventCreateComponent } from './event-create.component';
import { TagMultiselectModule } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { DropzoneWrapperModule } from 'src/app/feature/reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module';
import { TextInputModule } from 'src/app/feature/reusable/inputs/fields/text-input/text-input.module';
import { SelectModule } from 'src/app/feature/reusable/inputs/select/select.module';
import { DatepickerFromToModule } from '../../datepicker-from-to/datepicker-from-to.module';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { RichTextEditorModule } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { LocationPickerModule } from 'src/app/feature/reusable/ui-elements/location-picker/location-picker.module';
import { PinActivityModule } from '../../pin-activity/pin-activity.module';
import { DropzoneExistingImagesModule } from 'src/app/feature/reusable/ui-elements/dropzone-existing-images/dropzone-existing-images.module';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [EventCreateComponent],
  imports: [
    CommonModule,
    TagMultiselectModule,
    DropzoneWrapperModule,
    TextInputModule,
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
  exports: [EventCreateComponent]
})
export class EventCreateModule { }
