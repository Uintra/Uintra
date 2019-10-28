import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FaqPanelComponent } from './faq-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [FaqPanelComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [ {provide: DYNAMIC_COMPONENT, useValue: FaqPanelComponent}],
  entryComponents: [FaqPanelComponent]
})
export class FaqPanelModule { }
