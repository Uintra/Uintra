import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PictureComponent } from './picture.component';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [PictureComponent],
  imports: [
    CommonModule,
    TranslateModule,
  ],
  exports: [PictureComponent]
})
export class PictureModule { }
