import { Component, Input, HostBinding, SecurityContext } from '@angular/core';
import { IAccordionPanel } from '../../../shared/interfaces/panels/faq/faq-panel.interface'
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'faq-panel',
  templateUrl: './faq-panel.html',
  styleUrls: ['./faq-panel.less'],
})
export class FaqPanel {
  @Input() data: IAccordionPanel;
  constructor(
    private sanitizer: DomSanitizer,
    ) { }

  @HostBinding('class') rootClasses;

  ngOnInit()
  {
    this.setUniqueToItems();

    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    `;
  }

  setUniqueToItems() {
    this.data.items = this.data.items.map(item => ({
      ...item,
      id: '_' + Math.random().toString(36).substr(2, 9)
    }));
  }

  getSanitizedDescription(descr) {
    return this.sanitizer.sanitize(SecurityContext.HTML, descr);
  }
}
