import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoPanelComponent } from './video-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [VideoPanelComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [{ provide: DYNAMIC_COMPONENT, useValue: VideoPanelComponent}],
  entryComponents: [VideoPanelComponent]
})
export class VideoPanelModule { }
