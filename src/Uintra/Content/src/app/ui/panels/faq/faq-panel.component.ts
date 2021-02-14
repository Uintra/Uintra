import { Component, Input, HostBinding, SecurityContext } from '@angular/core';
import {IAccordionItem, IAccordionPanel} from '../../../shared/interfaces/panels/faq/faq-panel.interface';
import { DomSanitizer } from '@angular/platform-browser';
import {Router} from '@angular/router';

@Component({
  selector: 'faq-panel',
  templateUrl: './faq-panel.html',
  styleUrls: ['./faq-panel.less'],
})
export class FaqPanel {
  @Input() data: IAccordionPanel;
  activeAnchor: string;

  constructor(
    private sanitizer: DomSanitizer,
    private router: Router
    ) { }

  @HostBinding('class') rootClasses;

  ngOnInit() {
    this.activeAnchor = this.router.url.split('#')[1] ? this.router.url.split('#')[1] : null;
    this.setUniqueToItems();
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    `;
  }

  setUniqueToItems() {
    this.data.items = this.data.items.map(item => ({
      ...item,
      id: '_' + Math.random().toString(36).substr(2, 9),
      isChecked: item.anchor === this.activeAnchor
    }));
  }

  getSanitizedDescription(descr) {
    return this.sanitizer.sanitize(SecurityContext.HTML, descr);
  }

  toggleActive(item: IAccordionItem): void {
    item.isChecked = !item.isChecked;
  }
}
