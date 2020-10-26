import { Component, OnInit, HostBinding } from '@angular/core';
import { IQuotePanel } from '../../../shared/interfaces/panels/quote/quote-panel.interface';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'quote-panel',
  templateUrl: './quote-panel.html',
  styleUrls: ['./quote-panel.less']
})
export class QuotePanel implements OnInit{
  data: IQuotePanel;
  sanitizedContent: SafeHtml;

  @HostBinding('class') rootClasses;

  constructor(
    private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.sanitizedContent = this.sanitizer.bypassSecurityTrustHtml(this.data.quote.toString());
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    `;
  }
}
