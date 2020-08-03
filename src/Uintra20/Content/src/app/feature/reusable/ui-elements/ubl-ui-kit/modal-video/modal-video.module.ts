import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalVideoComponent } from './modal-video.component';
import { IframeComponent } from './components/iframe/iframe.component';


@NgModule({
  declarations: [ModalVideoComponent, IframeComponent],
  imports: [
    CommonModule,
  ],
  exports: [ModalVideoComponent]
})
export class ModalVideoModule { }
