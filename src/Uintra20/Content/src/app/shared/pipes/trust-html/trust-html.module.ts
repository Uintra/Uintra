import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrustHtmlPipe } from './trust-html.pipe';

@NgModule({
  declarations: [TrustHtmlPipe],
  imports: [
    CommonModule
  ], 
  exports: [TrustHtmlPipe]
})
export class TrustHtmlModule { }
