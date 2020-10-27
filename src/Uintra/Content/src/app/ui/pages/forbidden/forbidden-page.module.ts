import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { ForbiddenPage } from './forbidden-page.component';

@NgModule({
  declarations: [ForbiddenPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: ForbiddenPage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [ForbiddenPage]
})
export class ForbiddenPageModule {}
