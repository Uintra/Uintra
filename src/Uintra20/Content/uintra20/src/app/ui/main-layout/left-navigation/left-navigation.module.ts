import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LeftNavigationComponent } from "./left-navigation.component";
import { RouterModule } from "@angular/router";
import { CookieService } from "ngx-cookie-service";
import { SharedLinksComponent } from './components/shared-links/shared-links.component';
import { UlinkModule } from 'src/app/services/pipes/link/ulink.module';

@NgModule({
  declarations: [LeftNavigationComponent, SharedLinksComponent],
  imports: [CommonModule, RouterModule, UlinkModule],
  providers: [CookieService],
  exports: [LeftNavigationComponent]
})
export class LeftNavigationModule {}
