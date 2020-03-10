import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { PageNotFoundPage } from './page-not-found-page.component';

@NgModule({
  declarations: [PageNotFoundPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: PageNotFoundPage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [PageNotFoundPage]
})
export class PageNotFoundPageModule {}