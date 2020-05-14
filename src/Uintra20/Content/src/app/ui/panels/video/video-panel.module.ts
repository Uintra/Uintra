import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { VideoPanel } from './video-panel.component';
import { VideoPanelPopUpComponent } from './components/video-panel-pop-up/video-panel-pop-up.component';
import { PictureModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/picture/picture.module';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { TranslateModule } from '@ngx-translate/core';
import { IframeComponent } from './components/iframe/iframe.component';
import { LayoutModule } from '@angular/cdk/layout';

@NgModule({
  declarations: [VideoPanel, VideoPanelPopUpComponent, IframeComponent],
  imports: [
    CommonModule,
    PictureModule,
    ClickOutsideModule,
    TranslateModule,
    LayoutModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: VideoPanel}, ModalService],
  entryComponents: [VideoPanel, VideoPanelPopUpComponent]
})
export class VideoPanelModule {}