import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { CentralFeedPanel } from './central-feed-panel.component';

@NgModule({
  declarations: [CentralFeedPanel],
  imports: [
    CommonModule,
    NotImplementedModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: CentralFeedPanel}],
  entryComponents: [CentralFeedPanel]
})
export class CentralFeedPanelModule {}