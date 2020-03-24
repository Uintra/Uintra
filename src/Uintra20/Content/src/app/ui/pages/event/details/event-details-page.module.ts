import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { UbaselineCoreModule } from "@ubaseline/next";
import { UlinkModule } from "src/app/shared/pipes/link/ulink.module";
import { LikeButtonModule } from 'src/app/feature/reusable/ui-elements/like-button/like-button.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { CommentsModule } from 'src/app/feature/reusable/ui-elements/comments/comments.module';
import { DetailasHeaderModule } from 'src/app/feature/specific/activity/details/detailas-header/detailas-header.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { AttachmentsModule } from 'src/app/feature/specific/activity/details/attachments/attachments.module';
import { TranslateModule } from '@ngx-translate/core';
import { EventDetailsPage } from './event-details-page.component';
import { EventSubscriptionModule } from "../../../../feature/specific/activity/event-subscription/event-subscription.module";
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';

@NgModule({
  declarations: [EventDetailsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: EventDetailsPage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    LikeButtonModule,
    UserAvatarModule,
    CommentsModule,
    DetailasHeaderModule,
    UlinkModule,
    GroupDetailsWrapperModule,
    AttachmentsModule,
    TranslateModule,
    EventSubscriptionModule,
  ],
  providers: [],
  entryComponents: [EventDetailsPage]
})
export class EventDetailsPageModule {}
