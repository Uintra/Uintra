import { Component, OnInit, Input, HostBinding } from '@angular/core';

interface IProperty<T> {
  value: T;
}
interface ArticleStartPanelData {
  description?: IProperty<String>,
  title?: IProperty<String>;
  panelSettings?: any;
}
@Component({
  selector: 'app-article-start-panel',
  templateUrl: './article-start-panel.component.html',
  styleUrls: ['./article-start-panel.component.less']
})
export class ArticleStartPanelComponent implements OnInit {
  @Input() data: ArticleStartPanelData;
  @HostBinding('class') rootClasses;
  
  constructor() { }

  ngOnInit() {
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
      ${ this.data.panelSettings.behaviour.value || 'full-content' }
    `;
  }
}
