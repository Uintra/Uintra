import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavNotificationsComponent } from './nav-notifications.component';
import { ClickOutsideDirective } from '../../reusable/inputs/tag-multiselect/helpers/click-outside.directive';



@NgModule({
  declarations: [NavNotificationsComponent, ClickOutsideDirective],
  imports: [
    CommonModule
  ],
  exports: [ NavNotificationsComponent ]
})
export class NavNotificationsModule { }
