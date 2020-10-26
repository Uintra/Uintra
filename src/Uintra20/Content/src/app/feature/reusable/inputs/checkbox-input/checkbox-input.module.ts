import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckboxInputComponent } from './checkbox-input.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [CheckboxInputComponent],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    CheckboxInputComponent
  ]
})
export class CheckboxInputModule { }
