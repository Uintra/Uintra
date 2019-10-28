import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpotPanelComponent } from './spot-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';
import { NoImageComponent } from './component/no-image/no-image.component';
import { DefaultComponent } from './component/default/default.component';
import { SingleComponent } from './component/single/single.component';
import { SliderComponent } from './component/slider/slider.component';

@NgModule({
  declarations: [SpotPanelComponent, NoImageComponent, DefaultComponent, SingleComponent, SliderComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [{ provide: DYNAMIC_COMPONENT, useValue: SpotPanelComponent}],
  entryComponents: [SpotPanelComponent]
})
export class SpotPanelModule { }
