import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserNavigationComponent } from './user-navigation.component';
import { ClickOutsideModule } from '../../reusable/directives/click-outside/click-outside.module';



@NgModule({
  declarations: [ UserNavigationComponent],
  imports: [
    CommonModule,
    ClickOutsideModule
  ],
  exports: [
    UserNavigationComponent
  ]
})
export class UserNavigationModule { }
