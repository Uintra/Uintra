import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeftNavigationComponent } from './left-navigation.component';
import { RouterModule } from '@angular/router';
import { FirstOrderItemComponent } from './first-order-item/first-order-item.component';
import { SecondOrderItemComponent } from './second-order-item/second-order-item.component';


@NgModule({
  declarations: [LeftNavigationComponent, FirstOrderItemComponent, SecondOrderItemComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    LeftNavigationComponent
  ]
})
export class LeftNavigationModule { }
