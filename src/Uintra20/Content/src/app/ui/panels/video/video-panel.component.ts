import { Component, ViewEncapsulation, HostBinding } from '@angular/core';
import { IVideoPanel, IVideoViewModel, IVideoPickerVideoData } from '../../../shared/interfaces/panels/video/video-panel.interface';
import { resolveThemeCssClass } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/helpers/panel-settings';
import { MqService, config } from 'src/app/shared/services/general/mq.service';
import { ThumbnailBuilderService } from './service/thumbnail-builder.service';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';
import { Subscription } from 'rxjs';
import { ModalVideoComponent } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/modal-video/modal-video.component';

@Component({
  selector: 'video-panel',
  templateUrl: './video-panel.html',
  styleUrls: ['./video-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class VideoPanel {
  data: IVideoPanel;
  @HostBinding('class') hostClasses;

  videoData: IVideoViewModel;
  isShow: boolean = false;

  constructor(
    private thumbnailBuilder: ThumbnailBuilderService,
    private mq: MqService,
    private modalService: ModalService,
    private bpObserver: BreakpointObserver,
  ) { }

  ngOnInit() {
    this.hostClasses = resolveThemeCssClass(this.data.panelSettings);
    this.mobileDesktop(this.mobile.bind(this), this.desktop.bind(this));
  }

  mobile() {
    this.videoData = this.prepareVm(this.data && this.data.video && this.data.video.mobile);
  }
  desktop() {
    this.videoData = this.prepareVm(this.data && this.data.video && this.data.video.desktop);
  }

  open() {
    this.modalService.appendComponentToBody(ModalVideoComponent, {data: this.videoData})
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

  mqFactory(mediaQuery: string)
  {
    return (left: () => void, right: () => void): Subscription => {
      return this.bpObserver
        .observe(mediaQuery)
        .subscribe((state: BreakpointState) => {
          return state.matches ? right() : left();
        });
    }
  }

  mobileDesktop(mobile: () => void, desktop: () => void)
  {
    return this.mqFactory(`(min-width: ${config.minWidthTablet}px)`)(mobile, desktop);
  }
}