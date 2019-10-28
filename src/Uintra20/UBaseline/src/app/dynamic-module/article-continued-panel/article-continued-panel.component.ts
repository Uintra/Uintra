import { Component, OnInit, Input, HostBinding } from '@angular/core';

interface IProperty<T> {
  value: T;
}
interface ArticleContinuedPanelData {
  description?: IProperty<String>,
  title?: IProperty<string>;
  link?: IProperty<Object>;
  panelSettings?: any;
}
@Component({
  selector: 'app-article-continued-panel',
  templateUrl: './article-continued-panel.component.html',
  styleUrls: ['./article-continued-panel.component.less']
})
export class ArticleContinuedPanelComponent implements OnInit {
  @Input() data: ArticleContinuedPanelData;
  @HostBinding('class') rootClasses;

  constructor() { }

  ngOnInit() {
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
      ${ this.data.panelSettings.behaviour.value || 'full-content' }
    `;
  }
}
