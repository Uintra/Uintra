import { Component, OnInit, Input, HostBinding } from '@angular/core';

export interface IPictureData {
  alt: string;
  height: number;
  width: number;
  src: string;
  sources: IPictureSource[]
}
interface IPictureSource {
  media: string;
  srcSet: string;
}

@Component({
  selector: 'app-picture',
  templateUrl: './picture.component.html',
  styleUrls: ['./picture.component.less']
})
export class PictureComponent implements OnInit {
  @Input() data: IPictureData;
  @Input() scale: number = 100;
  @Input() ratio: number;
  @HostBinding('class') get className() { return this.isSvg() ? 'picture--svg': 'picture'};
  isIE11: boolean = false;
  style: any;

  constructor() { }

  ngOnInit()
  {
    if (!this.data) return;

    this.isIE11 = this.checkIE11();
    this.updateInlineStyles();
  }

  updateInlineStyles()
  {
    let width = 'auto';
    let maxWidth = '100%';

    if (this.scale < 100 && !this.isSvg())
    {
      width = `${this.scale}%`;
    }
    if (this.isSvg())
    {
      width = `${this.scale / 3}%`;
    }
    this.style = { width, maxWidth };
  }

  isSvg()
  {
    return !!this.data.src.match(/\.svg$/);
  }

  checkIE11() : boolean {
    const ua = window.navigator.userAgent;
    const trident = ua.indexOf('Trident/');

    if (trident > 0) {
      var rv = ua.indexOf('rv:');
      return !!parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }
  }
}