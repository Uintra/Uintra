import { Component, OnInit, Input } from '@angular/core';
import { IQuoteData } from './quote/quote.component';
import { IUProperty, UProperty } from '../../core/interface/umbraco-property';

export interface IQuotePanelData extends Object{
  author: IUProperty<string>;
  quote: IUProperty<string>;
  description: IUProperty<string>
}

@Component({
  selector: 'app-quote-panel',
  templateUrl: './quote-panel.component.html',
  styleUrls: ['./quote-panel.component.less']
})
export class QuotePanelComponent implements OnInit {
  @Input() data: IQuotePanelData;
  quoteData: IQuoteData;

  constructor() { }

  ngOnInit()
  {
    this.quoteData = UProperty.extract(this.data as any, ['author', 'quote', 'description']);
  }
}
