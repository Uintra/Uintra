import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from './button.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [ButtonComponent],
  imports: [
    CommonModule,
    RouterModule,
  ],
  exports: [ButtonComponent]
})
export class ButtonModule { }
