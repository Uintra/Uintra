import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { resolveThemeCssClass } from '../lib/helper/panel-settings';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import get  from 'lodash/get';
import { MqService } from 'src/app/service/mq.service';
import { IPictureData } from 'src/app/shared/components/picture/picture.component';
import { ThumbnailBuilderService } from './service/thumbnail-builder.service';

interface VideoPanelData {
  title?: IUProperty<string>;
  description?: IUProperty<string>;
  video?: IUProperty<IVideoPickerData>;
  panelSettings?: any;
}
interface IVideoPickerData {
  desktop?: IVideoPickerVideoData;
  mobile?: IVideoPickerVideoData;
}
export interface IVideoPickerVideoData {
  loop: boolean;
  thumbnail: string;
  type: number;
  url: string;
  videoCode: string;
}
export interface IVideoViewModel {
  video: object;
  thumbnail: IPictureData;
  title: IUProperty<string>;
  description: IUProperty<string>;
  params: string;
}

@Component({
  selector: 'app-video-panel',
  templateUrl: './video-panel.component.html',
  styleUrls: ['./video-panel.component.less']
})
export class VideoPanelComponent implements OnInit {
  @Input() data: VideoPanelData;
  @HostBinding('class') get hostClasses() {return resolveThemeCssClass(this.data.panelSettings)}

  videoData: IVideoViewModel;
  isShow: boolean = false;

  constructor(
    private thumbnailBuilder: ThumbnailBuilderService,
    private mq: MqService
  ) { }

  ngOnInit() {
    this.mq.mobileDesktop(this.mobile.bind(this), this.desktop.bind(this));
  }

  mobile() {
    this.videoData = this.prepareVm(get(this.data, 'video.value.mobile'));
  }
  desktop() {
    this.videoData = this.prepareVm(get(this.data, 'video.value.desktop'));
  }

  open() {
    this.isShow = true;
  }
  close() {
    this.isShow = false;
  }

  private prepareVm(data: IVideoPickerVideoData): IVideoViewModel
  {
    if (!data) return null;

    return {
      thumbnail: this.thumbnailBuilder.create(data.thumbnail),
      title: this.data.title,
      description: this.data.description,
      params: `?controls=1&muted=0&autoplay=1&showinfo=0`,
      video: {
        url: data.url,
        type: data.type,
      }
    };
  }
}
