import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LeftNavigationComponent } from "./left-navigation.component";
import { RouterModule } from "@angular/router";
import { CookieService } from "ngx-cookie-service";

@NgModule({
  declarations: [LeftNavigationComponent],
  imports: [CommonModule, RouterModule],
  providers: [CookieService],
  exports: [LeftNavigationComponent]
})
export class LeftNavigationModule {}
