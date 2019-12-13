import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { TextInputComponent } from './text-input.component';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [CommonModule, FormsModule],
  declarations: [TextInputComponent],
  exports: [TextInputComponent]
})
export class TextInputModule {}
