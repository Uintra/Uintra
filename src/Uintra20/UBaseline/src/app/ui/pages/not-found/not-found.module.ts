import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotFoundComponent } from './not-found.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [{path: "", component: NotFoundComponent}];
@NgModule({
  declarations: [NotFoundComponent],
  imports: [
    RouterModule.forChild(routes),
    CommonModule
  ]
})
export class NotFoundModule { }
