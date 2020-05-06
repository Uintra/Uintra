import { Component, ViewEncapsulation } from '@angular/core';
import { IQuotePanel } from '../../../shared/interfaces/panels/quote/quote-panel.interface';

@Component({
  selector: 'quote-panel',
  templateUrl: './quote-panel.html',
  styleUrls: ['./quote-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class QuotePanel {
  data: IQuotePanel;
}