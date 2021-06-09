import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AS_DYNAMIC_COMPONENT, UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { PageNotFoundPage } from './page-not-found-page.component';

@NgModule({
  declarations: [PageNotFoundPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: PageNotFoundPage}]),
    UbaselineCoreModule,
  ],
  providers: [{ provide: AS_DYNAMIC_COMPONENT, useValue: PageNotFoundPage }],
  entryComponents: [PageNotFoundPage]
})
export class PageNotFoundPageModule {}
