import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { ForbiddenPage } from './forbidden-page.component';
import { AuthenticatedLayoutModule } from '../../main-layout/authenticated-layout/authenticated-layout.module';

@NgModule({
  declarations: [ForbiddenPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: ForbiddenPage}]),
    UbaselineCoreModule,
    AuthenticatedLayoutModule
  ],
  entryComponents: [ForbiddenPage]
})
export class ForbiddenPageModule {}
