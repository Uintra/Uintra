import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { CommentsPanel } from './comments-panel.component';
import { CommentsModule } from 'src/app/feature/reusable/ui-elements/comments/comments.module';

@NgModule({
  declarations: [CommentsPanel],
  imports: [CommonModule, CommentsModule],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: CommentsPanel}],
  entryComponents: [CommentsPanel]
})
export class CommentsPanelModule {}
