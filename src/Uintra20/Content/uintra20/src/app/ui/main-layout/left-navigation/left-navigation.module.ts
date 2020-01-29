import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LeftNavigationComponent } from "./left-navigation.component";
import { RouterModule } from "@angular/router";
import { CookieService } from "ngx-cookie-service";
import { SharedLinksComponent } from "./components/shared-links/shared-links.component";
import { UlinkModule } from "src/app/services/pipes/link/ulink.module";
import { MyLinksComponent } from "./components/my-links/my-links.component";
import { DragulaModule } from "ng2-dragula";

@NgModule({
  declarations: [
    LeftNavigationComponent,
    SharedLinksComponent,
    MyLinksComponent
  ],
  imports: [CommonModule, RouterModule, UlinkModule, DragulaModule.forRoot()],
  providers: [CookieService],
  exports: [LeftNavigationComponent]
})
export class LeftNavigationModule {}
