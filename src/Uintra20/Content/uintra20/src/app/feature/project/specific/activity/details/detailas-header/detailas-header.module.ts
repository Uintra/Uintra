import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DetailasHeaderComponent } from "./detailas-header.component";
import { UserAvatarModule } from "src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module";
import { RouterModule } from "@angular/router";

@NgModule({
  declarations: [DetailasHeaderComponent],
  imports: [CommonModule, UserAvatarModule, RouterModule],
  exports: [DetailasHeaderComponent]
})
export class DetailasHeaderModule {}
