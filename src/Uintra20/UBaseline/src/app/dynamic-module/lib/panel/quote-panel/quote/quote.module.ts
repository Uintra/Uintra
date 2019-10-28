import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuoteComponent } from './quote.component';

@NgModule({
  declarations: [QuoteComponent],
  imports: [
    CommonModule
  ],
  exports: [QuoteComponent]
})
export class QuoteModule { }
