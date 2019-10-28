import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContactPanelComponent } from './contact-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [ContactPanelComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [ {provide: DYNAMIC_COMPONENT, useValue: ContactPanelComponent}],
  entryComponents: [ContactPanelComponent]
})
export class ContactPanelModule { }
