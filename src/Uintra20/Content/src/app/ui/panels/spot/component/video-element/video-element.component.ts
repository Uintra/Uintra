import { Component, OnInit, Input } from '@angular/core';
import { IDefaultSpotData, IModalVideo } from 'src/app/shared/interfaces/panels/spot/spot-panel.interface';
import { MqService } from 'src/app/shared/services/general/mq.service';
import { IPictureData } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/picture';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { ModalVideoComponent } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/modal-video/modal-video.component';

@Component({
  selector: 'app-video-element',
  templateUrl: './video-element.component.html',
  styleUrls: ['./video-element.component.less']
})
export class VideoElementComponent implements OnInit {
  @Input() data: IDefaultSpotData;

  showingItem: IModalVideo = null;
  isShow: boolean;
  isMobile: boolean;

  constructor(
    private mq: MqService,
    private modalService: ModalService,
  ) { }

  ngOnInit() {
    this.mq.mobileDesktop(this.mobile.bind(this), this.desktop.bind(this));
  }

  mobile() {
    this.isMobile = true;
  }
  desktop() {
    this.isMobile = false;
  }

  getThumbnail(item: IDefaultSpotData) {
    return this.isMobile 
      ? item && item.media && item.media.video && ((item.media.video.mobile && item.media.video.mobile.thumbnail) || (item.media.video.desktop && item.media.video.desktop.thumbnail))
      : item && item.media && item.media.video && ((item.media.video.desktop && item.media.video.desktop.thumbnail) || (item.media.video.mobile && item.media.video.mobile.thumbnail));
  }

  open(item: IDefaultSpotData) {
    this.showingItem = this.modalItemBuilder(item);
    this.modalService.appendComponentToBody(ModalVideoComponent, {data: this.showingItem});
  }

  modalItemBuilder(item: IDefaultSpotData) {
    return {
      title: item.title,
      description: item.description,
      video: this.isMobile 
        ? item && item.media && item.media.video && (item.media.video.mobile || item.media.video.desktop)
        : item && item.media && item.media.video && (item.media.video.desktop || item.media.video.mobile)
    };
  }

  // Remove it after app-picture supports string parameter
  createThumbnailData(thumbnail: string): IPictureData {
    return {
      alt: thumbnail,
      height: 900,
      width: 1600,
      src: thumbnail,
      sources: [
        {
          media: ``,
          srcSet: `${thumbnail}?center=0.5,0.5&mode=crop&width=570&height=300`
        },
        {
          media: `(max-width: 1024px)`,
          srcSet: `${thumbnail}?center=0.5,0.5&mode=crop&width=330&height=176`
        },
        {
          media: `(max-width: 860px)`,
          srcSet: `${thumbnail}?center=0.5,0.5&mode=crop&width=700&height=360`
        }
      ]
    };
  }
}
