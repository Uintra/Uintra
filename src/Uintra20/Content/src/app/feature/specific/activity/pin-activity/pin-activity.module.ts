import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PinActivityComponent } from './pin-activity.component';
import { CheckboxInputModule } from '../../../reusable/inputs/checkbox-input/checkbox-input.module';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [PinActivityComponent],
  imports: [
    CommonModule,
    CheckboxInputModule,
    SqDatetimepickerModule,
    FormsModule,
    TranslateModule,
  ],
  exports: [ PinActivityComponent ]
})
export class PinActivityModule { }
