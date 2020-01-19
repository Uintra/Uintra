import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommentsComponent } from './comments.component';
import { RichTextEditorModule } from '../../inputs/rich-text-editor/rich-text-editor.module';
import { UserAvatarModule } from '../user-avatar/user-avatar.module';
import { LikeButtonModule } from '../like-button/like-button.module';
import { FormsModule } from '@angular/forms';
import { CommentItemComponent } from './components/comment-item/comment-item.component';
import { SubcommentItemComponent } from './components/subcomment-item/subcomment-item.component';
import { CommentHeaderComponent } from './components/comment-header/comment-header.component';

@NgModule({
  declarations: [
    CommentsComponent,
    CommentItemComponent,
    SubcommentItemComponent,
    CommentHeaderComponent
  ],
  imports: [
    CommonModule,
    RichTextEditorModule,
    UserAvatarModule,
    LikeButtonModule,
    FormsModule
  ],
  exports: [
    CommentsComponent
  ]
})
export class CommentsModule { }

