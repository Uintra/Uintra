import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconPanelComponent } from './icon-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';
import { RouterModule } from '@angular/router';
import { IconPanelItemComponent } from './component/icon-panel-item/icon-panel-item.component';

@NgModule({
  declarations: [IconPanelComponent, IconPanelItemComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule
  ],
  providers: [ {provide: DYNAMIC_COMPONENT, useValue: IconPanelComponent}],
  entryComponents: [IconPanelComponent]
})
export class IconPanelModule { }
