import { Component, OnInit, Input, HostBinding } from '@angular/core';

interface IProperty<T> {
  value: T;
}
interface HeroPanelData {
  title?: IProperty<string>;
  description?: IProperty<string>;
  media?: IProperty<any>;
  link?: IProperty<Object>;
  panelSettings?: any;
}
enum typeAliases {
  imageItem = "imageItem",
  videoItem = "videoItem"
}
@Component({
  selector: 'app-hero-panel',
  templateUrl: './hero-panel.component.html',
  styleUrls: ['./hero-panel.component.less']
})
export class HeroPanelComponent implements OnInit {
  @Input() data: HeroPanelData;
  @HostBinding('class') rootClasses;
  hasImage: boolean;
  media: any;

  constructor() { }

  ngOnInit() 
  {
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
      ${ this.data.panelSettings.behaviour.value || 'full-content' }
    `;
    
    this.setMedia();
  }

  private setMedia() {
    const media = this.data.media.value;

    if(!media) return;

    this.hasImage = !!media.image;

    if(this.hasImage) {
      this.media = media.image;
    }
    else {
      this.media = {
        desktop: media.video.desktop || media.video.mobile,
        mobile: media.video.mobile || media.video.desktop
      }
    }
  }
}

