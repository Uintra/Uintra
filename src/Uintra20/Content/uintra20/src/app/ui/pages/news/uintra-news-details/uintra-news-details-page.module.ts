import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { UbaselineCoreModule } from "@ubaseline/next";
import { UintraNewsDetailsPage } from "./uintra-news-details-page.component";
import { LikeButtonModule } from "src/app/feature/project/reusable/ui-elements/like-button/like-button.module";
import { UserAvatarModule } from "src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module";
import { CommentsModule } from "src/app/feature/project/reusable/ui-elements/comments/comments.module";
import { DetailasHeaderModule } from "src/app/feature/project/specific/activity/details/detailas-header/detailas-header.module";
import { UlinkModule } from "src/app/services/pipes/link/ulink.module";
import { GroupDetailsWrapperModule } from 'src/app/feature/project/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { AttachmentsModule } from 'src/app/feature/project/specific/activity/details/attachments/attachments.module';

@NgModule({
  declarations: [UintraNewsDetailsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: UintraNewsDetailsPage }]),
    UbaselineCoreModule,
    LikeButtonModule,
    UserAvatarModule,
    CommentsModule,
    DetailasHeaderModule,
    UlinkModule,
    GroupDetailsWrapperModule,
    AttachmentsModule,
  ],
  providers: [],
  entryComponents: [UintraNewsDetailsPage]
})
export class UintraNewsDetailsPageModule {}
