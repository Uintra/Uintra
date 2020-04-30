import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PopUpComponent } from './pop-up.component';
import { ClickOutsideModule } from '../../directives/click-outside/click-outside.module';



@NgModule({
  declarations: [PopUpComponent],
  imports: [
    CommonModule,
    ClickOutsideModule,
  ],
  exports: [PopUpComponent]
})
export class PopUpModule { }
