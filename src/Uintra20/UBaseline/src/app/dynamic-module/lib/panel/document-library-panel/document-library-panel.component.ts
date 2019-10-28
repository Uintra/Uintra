import { Component } from '@angular/core';
import { IButtonData } from 'src/app/shared/components/button/button.component';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { Html } from 'src/app/shared/interface/alias';
import { IDocumentLibraryData } from './document-library/document-library.component';
import { Link } from '../../core/interface/link';
import { UProperty } from '../../core/interface/umbraco-property';

interface IDocumentLibraryPanelData {
  linkForHeadline: IUProperty<IButtonData>;
  maxCountOfDocuments: IUProperty<number>;
  panelType: IUProperty<'Vertical' | 'Horizontal'>;
  richTextEditor: IUProperty<Html>;
  seeAllLink: IUProperty<IButtonData>;
  title: IUProperty<string>;
  mediaLibrary: IUProperty<IMediaLibraryItem[]>
}
interface IMediaLibraryItem {
  umbracoBytes: IUProperty<number>;
  umbracoExtension: IUProperty<string>;
  umbracoFile: IUProperty<{alt: string, height: number, sources: [], src: string, width: number}>;
  umbracoHeight: IUProperty<number>;
  umbracoWidth: IUProperty<number>;
  url: string;
  name: string;
}
@Component({
  selector: 'app-document-library-panel',
  templateUrl: './document-library-panel.component.html',
  styleUrls: ['./document-library-panel.component.less']
})
export class DocumentLibraryPanelComponent {

  data: IDocumentLibraryPanelData;
  documentLibraryData: IDocumentLibraryData;

  ngOnInit()
  {
    if (this.data) this.documentLibraryData = this.map(this.data);
  }

  map(data: IDocumentLibraryPanelData)
  {
    let extracted = UProperty.extract<IDocumentLibraryPanelData, IDocumentLibraryData>(data,
      ['title', 'seeAllLink', 'richTextEditor', 'panelType', 'mediaLibrary', 'maxCountOfDocuments', 'linkForHeadline']);

      try {
        const maxCount = extracted.maxCountOfDocuments || 5;

        extracted.mediaLibrary = data.mediaLibrary.value.slice(0, maxCount).map(i => {
          let link = new Link(i.url, i.name, false, '_self');

          return {...link, ...{extension: i.umbracoExtension.value}};
        });

      } catch (err) { console.log(err)}

      try {
        extracted.linkForHeadline = Link.fromButtonData(extracted.linkForHeadline);
      } catch (err) { console.log(err)}

      try {
        extracted.seeAllLink = Link.fromButtonData(extracted.seeAllLink);
      } catch (err) { console.log(err)}

    return extracted;
  }
}
