import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { CommentsPanel } from './comments-panel.component';
import { RichTextEditorModule } from 'src/app/feature/project/reusable/inputs/rich-text-editor/rich-text-editor.module';

@NgModule({
  declarations: [CommentsPanel],
  imports: [
    CommonModule,
    NotImplementedModule,
    RichTextEditorModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: CommentsPanel}],
  entryComponents: [CommentsPanel]
})
export class CommentsPanelModule {}