import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserNavigationComponent } from './user-navigation.component';



@NgModule({
  declarations: [ UserNavigationComponent ],
  imports: [
    CommonModule
  ],
  exports: [
    UserNavigationComponent
  ]
})
export class UserNavigationModule { }
