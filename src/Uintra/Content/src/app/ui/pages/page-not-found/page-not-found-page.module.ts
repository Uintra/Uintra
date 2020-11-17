import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { PageNotFoundPage } from './page-not-found-page.component';
import { AuthenticatedLayoutModule } from '../../main-layout/authenticated-layout/authenticated-layout.module';

@NgModule({
  declarations: [PageNotFoundPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: PageNotFoundPage}]),
    UbaselineCoreModule,
    AuthenticatedLayoutModule
  ],
  entryComponents: [PageNotFoundPage]
})
export class PageNotFoundPageModule {}
