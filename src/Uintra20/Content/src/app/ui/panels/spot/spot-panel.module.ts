import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { SpotPanel } from './spot-panel.component';
import { DefaultComponent } from './component/default/default.component';
import { SingleComponent } from './component/single/single.component';
import { NoImageComponent } from './component/no-image/no-image.component';
import { PictureModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/picture/picture.module';
import { ButtonModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/button/button.module';
import { VideoElementComponent } from './component/video-element/video-element.component';
import { ModalVideoComponent } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/modal-video/modal-video.component';
import { ModalVideoModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/modal-video/modal-video.module';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { TrustHtmlModule } from 'src/app/shared/pipes/trust-html/trust-html.module';

@NgModule({
  declarations: [
    SpotPanel,
    DefaultComponent, 
    NoImageComponent, 
    SingleComponent, 
    VideoElementComponent],
  imports: [
    CommonModule,
    PictureModule,
    ButtonModule,
    ModalVideoModule,
    TrustHtmlModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: SpotPanel}, ModalService],
  entryComponents: [SpotPanel, ModalVideoComponent]
})
export class SpotPanelModule {}