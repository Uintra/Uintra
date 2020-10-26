import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { TablePanel } from './table-panel.component';
import { HeadingComponent } from './components/heading/heading.component';

@NgModule({
  declarations: [TablePanel, HeadingComponent],
  imports: [
    CommonModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: TablePanel}],
  entryComponents: [TablePanel]
})
export class TablePanelModule {}