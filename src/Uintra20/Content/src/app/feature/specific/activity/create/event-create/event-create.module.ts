import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventCreateComponent } from './event-create.component';



@NgModule({
  declarations: [EventCreateComponent],
  imports: [
    CommonModule
  ],
  exports: [EventCreateComponent]
})
export class EventCreateModule { }
