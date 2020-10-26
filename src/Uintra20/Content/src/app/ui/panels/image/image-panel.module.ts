import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { ImagePanel } from './image-panel.component';
import { PictureModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/picture/picture.module';

@NgModule({
  declarations: [ImagePanel],
  imports: [
    CommonModule,
    PictureModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: ImagePanel}],
  entryComponents: [ImagePanel]
})
export class ImagePanelModule {}
