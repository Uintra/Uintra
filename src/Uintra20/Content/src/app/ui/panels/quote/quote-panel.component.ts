import { Component, OnInit, HostBinding } from '@angular/core';
import { IQuotePanel } from '../../../shared/interfaces/panels/quote/quote-panel.interface';

@Component({
  selector: 'quote-panel',
  templateUrl: './quote-panel.html',
  styleUrls: ['./quote-panel.less']
})
export class QuotePanel implements OnInit{
  data: IQuotePanel;

  @HostBinding('class') rootClasses;

  ngOnInit() {
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    `;
  }
}
