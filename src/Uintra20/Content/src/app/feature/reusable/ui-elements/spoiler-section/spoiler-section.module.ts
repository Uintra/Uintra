import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpoilerSectionComponent } from './spoiler-section.component';



@NgModule({
  declarations: [SpoilerSectionComponent],
  imports: [
    CommonModule
  ],
  exports: [
    SpoilerSectionComponent
  ]
})
export class SpoilerSectionModule { }
