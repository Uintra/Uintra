import { Component, Input } from '@angular/core';

export interface IQuoteData {
  quote: string;
  author?: string;
  description?: string;
}
@Component({
  selector: 'ubl-quote',
  templateUrl: './quote.component.html',
  styleUrls: ['./quote.component.less']
})
export class QuoteComponent {
  @Input() data: IQuoteData;
}
