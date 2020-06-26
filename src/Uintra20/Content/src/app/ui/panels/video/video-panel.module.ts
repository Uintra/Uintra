import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { VideoPanel } from './video-panel.component';
import { PictureModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/picture/picture.module';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { TranslateModule } from '@ngx-translate/core';
import { LayoutModule } from '@angular/cdk/layout';
import { ModalVideoModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/modal-video/modal-video.module';
import { ModalVideoComponent } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/modal-video/modal-video.component';
import { TrustHtmlModule } from 'src/app/shared/pipes/trust-html/trust-html.module';

@NgModule({
  declarations: [VideoPanel],
  imports: [
    CommonModule,
    PictureModule,
    ClickOutsideModule,
    TranslateModule,
    LayoutModule,
    ModalVideoModule,
    TrustHtmlModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: VideoPanel}, ModalService],
  entryComponents: [VideoPanel, ModalVideoComponent]
})
export class VideoPanelModule {}