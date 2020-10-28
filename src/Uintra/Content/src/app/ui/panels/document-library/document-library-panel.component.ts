import { Component, OnInit, HostBinding } from "@angular/core";

import { IDocumentLibraryData } from "./document-library/document-library.component";
import {
  IUProperty,
  UProperty,
} from "src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/umbraco-property";
import { Link } from "src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/link";
import { Html } from "src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/types";
import { IButtonData } from "src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/button";
import { IPanelSettings } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/panel-settings';
import { DomSanitizer } from '@angular/platform-browser';

interface IDocumentLibraryPanelData {
  link: IButtonData;
  pageSize: number;
  listOrder: "Vertical" | "Horizontal";
  richTextEditor: Html;
  linkText: IButtonData;
  title: string;
  mediaLibrary: IMediaLibraryItem[];
  anchor: string;
  panelSettings?: IPanelSettings;
}
interface IMediaLibraryItem {
  umbracoBytes: number;
  umbracoExtension: string;
  umbracoFile: {
    alt: string;
    height: number;
    sources: [];
    src: string;
    width: number;
  };
  umbracoHeight: number;
  umbracoWidth: number;
  url: string;
  name: string;
}
@Component({
  selector: "app-document-library-panel",
  templateUrl: "./document-library-panel.component.html",
  styleUrls: ["./document-library-panel.component.less"],
})
export class DocumentLibraryPanelComponent implements OnInit {
  data: IDocumentLibraryPanelData;
  documentLibraryData: IDocumentLibraryData;

  @HostBinding('class') rootClasses;

  constructor(private sanitized: DomSanitizer) { }

  ngOnInit() {
    if (this.data) this.documentLibraryData = this.map(this.data);

    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    `;
  }

  map(data: IDocumentLibraryPanelData) {
    let extracted = UProperty.extract<
      IDocumentLibraryPanelData,
      IDocumentLibraryData
    >(data, [
      "title",
      "seeAllLink",
      "richTextEditor",
      "listOrder",
      "mediaLibrary",
      "pageSize",
      "linkForHeadline",
    ]);

    extracted.richTextEditor = this.sanitized.bypassSecurityTrustHtml(extracted.richTextEditor as string);

    try {
      const maxCount = data.pageSize || 5;

      extracted.mediaLibrary = data.mediaLibrary.slice(0, maxCount).map((i) => {
        let link = new Link(i.url, i.name, false, "_self");

        return { ...link, ...{ extension: i.umbracoExtension } };
      });
    } catch (err) {
      console.log(err);
    }

    try {
      if (data.linkText) {
        extracted.linkForHeadline = Link.fromButtonData(
          data.linkText
        );
      }

    } catch (err) {
      console.log(err);
    }

    try {
      if (data.link) {
        extracted.seeAllLink = Link.fromButtonData(data.link);
      }
    } catch (err) {
      console.log(err);
    }

    return extracted;
  }
}
