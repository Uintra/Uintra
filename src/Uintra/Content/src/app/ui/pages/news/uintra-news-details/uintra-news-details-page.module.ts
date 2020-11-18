import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { UbaselineCoreModule } from "ubaseline-next-for-uintra";
import { UintraNewsDetailsPage } from "./uintra-news-details-page.component";
import { UlinkModule } from "src/app/shared/pipes/link/ulink.module";
import { LikeButtonModule } from 'src/app/feature/reusable/ui-elements/like-button/like-button.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { CommentsModule } from 'src/app/feature/reusable/ui-elements/comments/comments.module';
import { DetailasHeaderModule } from 'src/app/feature/specific/activity/details/detailas-header/detailas-header.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { AttachmentsModule } from 'src/app/feature/specific/activity/details/attachments/attachments.module';
import { TranslateModule } from '@ngx-translate/core';


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
    TranslateModule,

  ],
  providers: [],
  entryComponents: [UintraNewsDetailsPage]
})
export class UintraNewsDetailsPageModule {}
