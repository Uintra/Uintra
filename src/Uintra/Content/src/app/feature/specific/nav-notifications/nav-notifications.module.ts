import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {NavNotificationsComponent} from './nav-notifications.component';
import {HttpClientModule} from '@angular/common/http';
import {UserAvatarModule} from '../../reusable/ui-elements/user-avatar/user-avatar.module';
import {RouterModule} from '@angular/router';
import {NotificationsItemComponent} from './notifications-item/notifications-item.component';
import {UlinkModule} from 'src/app/shared/pipes/link/ulink.module';
import {NotificationCountComponent} from './notification-count/notification-count.component';
import {PinActivityModule} from '../activity/pin-activity/pin-activity.module';
import {ClickOutsideModule} from 'src/app/shared/directives/click-outside/click-outside.module';
import {TranslateModule} from '@ngx-translate/core';
import {PopUpComponent} from "../../../shared/ui-elements/pop-up/pop-up.component";
import { PopUpModule } from 'src/app/shared/ui-elements/pop-up/pop-up.module';

@NgModule({
  declarations: [NavNotificationsComponent, NotificationsItemComponent, NotificationCountComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    UserAvatarModule,
    RouterModule,
    ClickOutsideModule,
    UlinkModule,
    PinActivityModule,
    TranslateModule,
    PopUpModule,
  ],
  exports: [NavNotificationsComponent, NotificationsItemComponent],
  entryComponents: [PopUpComponent]
})
export class NavNotificationsModule {
}
