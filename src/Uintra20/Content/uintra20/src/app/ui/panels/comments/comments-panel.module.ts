import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { CommentsPanel } from './comments-panel.component';
import { RichTextEditorModule } from 'src/app/feature/project/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { UserAvatarModule } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module';
import { LikeButtonModule } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.module';
import { FormsModule } from '@angular/forms';
import { CommentItemComponent } from './components/comment-item/comment-item.component';
import { SubcommentItemComponent } from './components/subcomment-item/subcomment-item.component';
import { CommentHeaderComponent } from './components/comment-header/comment-header.component';

@NgModule({
  declarations: [CommentsPanel, CommentItemComponent, SubcommentItemComponent, CommentHeaderComponent],
  imports: [
    CommonModule,
    NotImplementedModule,
    RichTextEditorModule,
    UserAvatarModule,
    LikeButtonModule,
    FormsModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: CommentsPanel}],
  entryComponents: [CommentsPanel]
})
export class CommentsPanelModule {}