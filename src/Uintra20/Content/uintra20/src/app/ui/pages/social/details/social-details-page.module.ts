import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AS_DYNAMIC_COMPONENT } from "@ubaseline/next";
import { RouterModule } from "@angular/router";
import { UbaselineCoreModule } from "@ubaseline/next";
import { SocialDetailsPanelComponent } from "./social-details-page.component";
import { LikeButtonModule } from "src/app/feature/project/reusable/ui-elements/like-button/like-button.module";
import { UserAvatarModule } from "src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module";
import { CommentsModule } from "src/app/feature/project/reusable/ui-elements/comments/comments.module";
import { DetailasHeaderModule } from "src/app/feature/project/specific/activity/details/detailas-header/detailas-header.module";
import { GroupDetailsWrapperModule } from 'src/app/feature/project/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { AttachmentsModule } from 'src/app/feature/project/specific/activity/details/attachments/attachments.module';

@NgModule({
  declarations: [SocialDetailsPanelComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: "", component: SocialDetailsPanelComponent }
    ]),
    UbaselineCoreModule,
    LikeButtonModule,
    UserAvatarModule,
    CommentsModule,
    DetailasHeaderModule,
    GroupDetailsWrapperModule,
    AttachmentsModule,
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: SocialDetailsPanelComponent }
  ],
  entryComponents: [SocialDetailsPanelComponent]
})
export class SocialDetailsPageModule {}
