import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PinActivityComponent } from './pin-activity.component';
import { CheckboxInputModule } from '../../../reusable/inputs/checkbox-input/checkbox-input.module';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [PinActivityComponent],
  imports: [
    CommonModule,
    CheckboxInputModule,
    SqDatetimepickerModule,
    FormsModule
  ],
  exports: [ PinActivityComponent ]
})
export class PinActivityModule { }
