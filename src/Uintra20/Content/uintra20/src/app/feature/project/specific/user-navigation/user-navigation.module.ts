import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserNavigationComponent } from './user-navigation.component';
import { ClickOutsideDirective } from './helpers/click-outside.directive';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [ UserNavigationComponent, ClickOutsideDirective],
  imports: [
    CommonModule,
  ],
  exports: [
    UserNavigationComponent
  ]
})
export class UserNavigationModule { }
