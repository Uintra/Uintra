import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { FaqPanel } from './faq-panel.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [FaqPanel],
  imports: [
    CommonModule,
    RouterModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: FaqPanel}],
  entryComponents: [FaqPanel]
})
export class FaqPanelModule {}