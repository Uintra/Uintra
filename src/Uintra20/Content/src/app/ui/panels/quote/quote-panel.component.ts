import { Component, OnInit, HostBinding, ViewEncapsulation } from '@angular/core';
import { IQuotePanel } from '../../../shared/interfaces/panels/quote/quote-panel.interface';

interface ArticleStartPanelData {
  panelSettings?: any;
}

@Component({
  selector: 'quote-panel',
  templateUrl: './quote-panel.html',
  styleUrls: ['./quote-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class QuotePanel implements OnInit{
  data: IQuotePanel;

  @HostBinding('class') rootClasses;

  ngOnInit() {
    // this.rootClasses = `
    //   ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    // `;
  }
}
