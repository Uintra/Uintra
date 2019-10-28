import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeroPanelComponent } from './hero-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [HeroPanelComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [ {provide: DYNAMIC_COMPONENT, useValue: HeroPanelComponent}],
  entryComponents: [HeroPanelComponent]
})
export class HeroPanelModule { }
