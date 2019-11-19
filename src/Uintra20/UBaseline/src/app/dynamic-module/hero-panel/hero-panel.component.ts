import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { resolveThemeCssClass } from '../lib/helper/panel-settings';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { IButtonData } from 'src/app/shared/components/button/button.component';
import { IPanelSettings } from 'src/app/shared/interface/panel-settings';

interface HeroPanelData {
  title: IUProperty<string>;
  description: IUProperty<string>;
  media: IUProperty<any>; // TODO: create appropriate interface
  link: IUProperty<IButtonData>;
  panelSettings: IPanelSettings;
}

@Component({
  selector: 'app-hero-panel',
  templateUrl: './hero-panel.component.html',
  styleUrls: ['./hero-panel.component.less']
})
export class HeroPanelComponent implements OnInit {
  @Input() data: Partial<HeroPanelData>;
  @HostBinding('class') get hostClasses() {return resolveThemeCssClass(this.data.panelSettings)}

  hasImage: boolean;
  media: any;

  ngOnInit()
  {
    this.prepareMedia();
  }

  private prepareMedia()
  {
    if (!this.data || !this.data.media || !this.data.media.value) return;

    const media = this.data.media.value;

    this.hasImage = !!media.image;

    if (this.hasImage)
    {
      this.media = media.image;
    } else {
      this.media = {
        desktop: media.video.desktop || media.video.mobile,
        mobile: media.video.mobile || media.video.desktop
      }
    }
  }
}

