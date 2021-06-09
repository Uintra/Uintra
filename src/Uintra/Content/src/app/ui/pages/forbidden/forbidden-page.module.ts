import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AS_DYNAMIC_COMPONENT, UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { ForbiddenPage } from './forbidden-page.component';

@NgModule({
  declarations: [ForbiddenPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: ForbiddenPage}]),
    UbaselineCoreModule,
  ],
  providers: [{ provide: AS_DYNAMIC_COMPONENT, useValue: ForbiddenPage }],
  entryComponents: [ForbiddenPage]
})
export class ForbiddenPageModule {}
