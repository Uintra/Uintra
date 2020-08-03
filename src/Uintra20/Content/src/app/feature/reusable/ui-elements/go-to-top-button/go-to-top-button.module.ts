import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GoToTopButtonComponent } from './go-to-top-button.component';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [ GoToTopButtonComponent ],
  imports: [
    CommonModule,
    TranslateModule
  ],
  exports: [
    GoToTopButtonComponent
  ],
})
export class GoToTopButtonModule { }
