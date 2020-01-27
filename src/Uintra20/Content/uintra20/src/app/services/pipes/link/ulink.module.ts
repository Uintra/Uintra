import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LinkPipe } from "./link.pipe";
import { ParamsPipe } from "./params.pipe";

@NgModule({
  declarations: [LinkPipe, ParamsPipe],
  imports: [CommonModule],
  exports: [LinkPipe, ParamsPipe]
})
export class UlinkModule {}
