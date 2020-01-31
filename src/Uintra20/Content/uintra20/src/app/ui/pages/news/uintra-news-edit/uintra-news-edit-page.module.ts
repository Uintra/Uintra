import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraNewsEditPage } from './uintra-news-edit-page.component';

@NgModule({
  declarations: [UintraNewsEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraNewsEditPage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [UintraNewsEditPage]
})
export class UintraNewsEditPageModule {}
