import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatepickerFromToComponent } from './datepicker-from-to.component';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [DatepickerFromToComponent],
  imports: [
    CommonModule,
    SqDatetimepickerModule,
    FormsModule,
    TranslateModule,
  ],
  exports: [DatepickerFromToComponent]
})
export class DatepickerFromToModule { }
