import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { CommentsPanel } from './comments-panel.component';

@NgModule({
  declarations: [CommentsPanel],
  imports: [
    CommonModule,
    NotImplementedModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: CommentsPanel}],
  entryComponents: [CommentsPanel]
})
export class CommentsPanelModule {}