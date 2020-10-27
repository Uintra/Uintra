import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { QuotePanel } from './quote-panel.component';

@NgModule({
  declarations: [QuotePanel],
  imports: [
    CommonModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: QuotePanel}],
  entryComponents: [QuotePanel]
})
export class QuotePanelModule {}