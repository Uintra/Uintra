import { Component, Input, HostBinding } from "@angular/core";
import { ILink } from "src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/link";
import { Html } from "src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/types";
import { SafeHtml } from '@angular/platform-browser';

export interface IDocumentLibraryData {
  linkForHeadline: ILink;
  pageSize: number;
  listOrder: "Vertical" | "Horizontal";
  richTextEditor: SafeHtml;
  seeAllLink: ILink;
  title: string;
  mediaLibrary: Array<ILink & { extension: string }>;
}
@Component({
  selector: "ubl-document-library",
  templateUrl: "./document-library.component.html",
  styleUrls: ["./document-library.component.less"],
})
export class DocumentLibraryComponent {
  @HostBinding("class") cssClass =
    "document-library document-library--vertical";
  @Input() data: IDocumentLibraryData;

  ngOnInit() {
    if (!this.data) return;

    const viewMode = this.data.listOrder;

    if (viewMode) {
      this.cssClass = `document-library document-library--${viewMode.toLowerCase()}`;
    }
  }
}
