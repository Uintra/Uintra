import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatepickerFromToComponent } from './datepicker-from-to.component';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [DatepickerFromToComponent],
  imports: [
    CommonModule,
    SqDatetimepickerModule,
    FormsModule
  ],
  exports: [DatepickerFromToComponent]
})
export class DatepickerFromToModule { }
