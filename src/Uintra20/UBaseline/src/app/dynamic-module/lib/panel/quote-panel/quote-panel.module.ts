import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuotePanelComponent } from './quote-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { QuoteModule } from './quote/quote.module';

@NgModule({
  declarations: [QuotePanelComponent],
  providers: [ {provide: DYNAMIC_COMPONENT, useValue: QuotePanelComponent}],
  imports: [
    CommonModule,
    QuoteModule
  ],
  entryComponents: [QuotePanelComponent]
})
export class QuotePanelModule {}