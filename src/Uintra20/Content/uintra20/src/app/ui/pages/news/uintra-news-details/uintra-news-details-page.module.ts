import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { UbaselineCoreModule } from "@ubaseline/next";
import { UintraNewsDetailsPage } from "./uintra-news-details-page.component";
import { LikeButtonModule } from "src/app/feature/project/reusable/ui-elements/like-button/like-button.module";
import { UserAvatarModule } from "src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module";
import { CommentsModule } from "src/app/feature/project/reusable/ui-elements/comments/comments.module";
import { PostHeaderComponent } from "./post-header/post-header.component";

@NgModule({
  declarations: [UintraNewsDetailsPage, PostHeaderComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: UintraNewsDetailsPage }]),
    UbaselineCoreModule,
    LikeButtonModule,
    UserAvatarModule,
    CommentsModule
  ],
  providers: [],
  entryComponents: [UintraNewsDetailsPage]
})
export class UintraNewsDetailsPageModule {}
