import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DetailasHeaderComponent } from "./detailas-header.component";
import { RouterModule } from "@angular/router";
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { UlinkModule } from "src/app/shared/pipes/link/ulink.module";

@NgModule({
  declarations: [DetailasHeaderComponent],
  imports: [CommonModule, UserAvatarModule, RouterModule,UlinkModule],
  exports: [DetailasHeaderComponent]
})
export class DetailasHeaderModule {}
