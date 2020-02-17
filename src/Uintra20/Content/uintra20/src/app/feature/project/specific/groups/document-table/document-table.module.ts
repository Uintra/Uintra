import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DocumentTableComponent } from "./document-table.component";

@NgModule({
  declarations: [DocumentTableComponent],
  imports: [CommonModule],
  exports: [DocumentTableComponent]
})
export class DocumentTableModule {}
