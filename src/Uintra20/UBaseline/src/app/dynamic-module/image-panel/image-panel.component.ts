import { Component, OnInit, Input, HostBinding } from '@angular/core';

interface IProperty<T> {
  value: T;
}
interface ILinkProperty {
  url: string;
  target: string;
}
interface ImagePanelData {
  image?: IProperty<string>;
  link?: IProperty<ILinkProperty>;
  panelSettings?: any;
}
@Component({
  selector: 'app-image-panel',
  templateUrl: './image-panel.component.html',
  styleUrls: ['./image-panel.component.less']
})
export class ImagePanelComponent implements OnInit {
  @Input() data: ImagePanelData;
  @HostBinding('class') rootClasses;

  constructor() { }

  ngOnInit() {
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
      ${ this.data.panelSettings.behaviour.value || 'full-content' }
    `;
  }
}
