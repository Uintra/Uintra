import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LeftNavigationComponent } from "./left-navigation.component";
import { RouterModule } from "@angular/router";
import { CookieService } from "ngx-cookie-service";
import { SharedLinksComponent } from "./components/shared-links/shared-links.component";
import { UlinkModule } from "src/app/shared/pipes/link/ulink.module";
import { MyLinksComponent } from "./components/my-links/my-links.component";
import { DragulaModule } from "ng2-dragula";
import { UserNavMobileComponent } from './components/user-nav-mobile/user-nav-mobile.component';
import { GroupsComponent } from './components/groups/groups.component';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    LeftNavigationComponent,
    SharedLinksComponent,
    MyLinksComponent,
    UserNavMobileComponent,
    GroupsComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    UlinkModule,
    DragulaModule.forRoot(),
    UserAvatarModule,
    TranslateModule,
  ],
  providers: [CookieService],
  exports: [LeftNavigationComponent]
})
export class LeftNavigationModule {}
