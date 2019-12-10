import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RadioLinkGroupComponent } from './radio-link-group.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [RadioLinkGroupComponent],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [ RadioLinkGroupComponent ]
})
export class RadioLinkGroupModule { }
