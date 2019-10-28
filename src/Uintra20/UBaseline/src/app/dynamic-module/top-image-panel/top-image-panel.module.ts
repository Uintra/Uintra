import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopImagePanelComponent } from './top-image-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';

@NgModule({
  declarations: [TopImagePanelComponent],
  imports: [
    CommonModule
  ],
  providers: [{ provide: DYNAMIC_COMPONENT, useValue: TopImagePanelComponent}],
  entryComponents: [TopImagePanelComponent]
})
export class TopImagePanelModule { }
