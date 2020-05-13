import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { LinksPanel } from './links-panel.component';
import { LinkModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/link/link.module';

@NgModule({
  declarations: [LinksPanel],
  imports: [
    CommonModule,
    LinkModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: LinksPanel}],
  entryComponents: [LinksPanel]
})
export class LinksPanelModule {}
