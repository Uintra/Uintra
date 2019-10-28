import { Component, Input, HostBinding } from '@angular/core';
import { get } from 'lodash';
import { ILink } from '../../../core/interface/link';
import { Html } from '../../../core/interface/types';

export interface IDocumentLibraryData {
  linkForHeadline: ILink;
  maxCountOfDocuments: number;
  panelType: 'Vertical' | 'Horizontal';
  richTextEditor: Html;
  seeAllLink: ILink;
  title: string;
  mediaLibrary: Array<ILink & {extension: string}>;
}
@Component({
  selector: 'ubl-document-library',
  templateUrl: './document-library.component.html',
  styleUrls: ['./document-library.component.less']
})
export class DocumentLibraryComponent {
  @HostBinding('class') cssClass = "document-library document-library--vertical";
  @Input() data: IDocumentLibraryData;

  ngOnInit()
  {
    if (!this.data) return;

    const viewMode = this.data.panelType;

    if (viewMode)
    {
      this.cssClass =`document-library document-library--${viewMode.toLowerCase()}`;
    }
  }
}
