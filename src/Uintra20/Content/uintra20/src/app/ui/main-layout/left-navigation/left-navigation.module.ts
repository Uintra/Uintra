import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeftNavigationComponent } from './left-navigation.component';



@NgModule({
  declarations: [LeftNavigationComponent],
  imports: [
    CommonModule
  ],
  exports: [
    LeftNavigationComponent
  ]
})
export class LeftNavigationModule { }
