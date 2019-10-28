import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImagePanelComponent } from './image-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [ImagePanelComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [{ provide: DYNAMIC_COMPONENT, useValue: ImagePanelComponent}],
  entryComponents: [ImagePanelComponent]
})
export class ImagePanelModule { }
