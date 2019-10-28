import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { DynamicComponentLoaderModule } from 'src/app/shared/dynamic-component-loader/dynamic-component-loader.module';

const routes: Routes = [{path: "", component: HomeComponent}];

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    SharedModule,
    DynamicComponentLoaderModule
  ]
})
export class HomeModule { }
