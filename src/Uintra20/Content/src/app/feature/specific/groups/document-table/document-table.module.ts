import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DocumentTableComponent } from "./document-table.component";
import { UserAvatarModule } from "../../../reusable/ui-elements/user-avatar/user-avatar.module";
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [DocumentTableComponent],
  imports: [
    CommonModule,
    UserAvatarModule,
    TranslateModule,
  ],
  exports: [DocumentTableComponent]
})
export class DocumentTableModule {}
