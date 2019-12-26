import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { LikesPanel } from './likes-panel.component';

@NgModule({
  declarations: [LikesPanel],
  imports: [
    CommonModule,
    NotImplementedModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: LikesPanel}],
  entryComponents: [LikesPanel]
})
export class LikesPanelModule {}