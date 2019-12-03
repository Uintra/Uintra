import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { HomePage } from './home-page.component';

@NgModule({
  declarations: [HomePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: HomePage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [HomePage]
})
export class HomePageModule {}