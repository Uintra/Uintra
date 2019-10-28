import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TablePanelComponent } from './table-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [TablePanelComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [{ provide: DYNAMIC_COMPONENT, useValue: TablePanelComponent}],
  entryComponents: [TablePanelComponent]
})
export class TablePanelModule { }
