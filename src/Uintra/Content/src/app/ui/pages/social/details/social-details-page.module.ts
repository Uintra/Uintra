import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AS_DYNAMIC_COMPONENT, UbaselineCoreModule } from "ubaseline-next-for-uintra";
import { RouterModule } from "@angular/router";
import { SocialDetailsPanelComponent } from "./social-details-page.component";
import { LikeButtonModule } from 'src/app/feature/reusable/ui-elements/like-button/like-button.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { CommentsModule } from 'src/app/feature/reusable/ui-elements/comments/comments.module';
import { DetailasHeaderModule } from 'src/app/feature/specific/activity/details/detailas-header/detailas-header.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { AttachmentsModule } from 'src/app/feature/specific/activity/details/attachments/attachments.module';
import { TranslateModule } from '@ngx-translate/core';
import { LinkPreviewModule } from 'src/app/feature/specific/link-preview/link-preview.module';
import { UlinkModule } from "src/app/shared/pipes/link/ulink.module";
import { AuthenticatedLayoutModule } from 'src/app/ui/main-layout/authenticated-layout/authenticated-layout.module';

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
    TranslateModule,
    LinkPreviewModule,
    UlinkModule,
    AuthenticatedLayoutModule
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: SocialDetailsPanelComponent }
  ],
  entryComponents: [SocialDetailsPanelComponent]
})
export class SocialDetailsPageModule {}
