import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectComponent } from './select.component';
import { TextInputModule } from '../fields/text-input/text-input.module';
import { ClickOutsideModule } from '../../directives/click-outside/click-outside.module';
import { NgSelect2Module } from 'ng-select2';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [SelectComponent],
  imports: [
    CommonModule,
    NgSelect2Module,
    TextInputModule,
    ClickOutsideModule,
    FormsModule
  ],
  exports: [SelectComponent]
})
export class SelectModule { }
