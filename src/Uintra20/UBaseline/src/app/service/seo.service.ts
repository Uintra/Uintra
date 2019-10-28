import { Injectable, Inject  } from '@angular/core';
import { Meta, MetaDefinition } from '@angular/platform-browser';
import { IUProperty } from '../shared/interface/umbraco-property';
import { DOCUMENT } from '@angular/common';
import { AppConfigService } from './app-config.service';

export interface IPageMetaData {
  metaDescription?: IUProperty<string>;
  metaTitle?: IUProperty<string>;
  socialDescription?: IUProperty<string>;
  socialImage?: IUProperty<string>;
  socialTitle?: IUProperty<string>;
}

@Injectable({
  providedIn: 'root'
})
export class SEOService {

  constructor(
    @Inject(DOCUMENT) private dom: Document,
    private meta: Meta,
    private appConfigService: AppConfigService) {}

  addTags(metaData: IPageMetaData)
  {
    const meta = this.mapServerModel(metaData);

    meta.forEach(metaItem =>
      {
        let selector = metaItem.name ? `name=${metaItem.name}` : `property='${metaItem.property}'`;
        this.meta.updateTag(metaItem, selector);
      });
  }

  addCanonicalURL(url: string)
  {
    let link: HTMLLinkElement = this.dom.querySelector('[rel="canonical"]');
    if(!link)
    {
      link = this.dom.createElement('link');
      link.setAttribute('rel', 'canonical');
      this.dom.head.appendChild(link);
    }

    let href = url.indexOf("://") > -1 ? url : this.appConfigService.getHostName() + url;
    link.setAttribute('href', href);
  }

  private mapServerModel(metaData: IPageMetaData)
  {
    let meta: MetaDefinition[] = [];

    if (!metaData) return meta;

    if (metaData.metaTitle) meta.push({ name: 'title', content: metaData.metaTitle.value || '' });
    if (metaData.metaDescription) meta.push({ name: 'description', content: metaData.metaDescription.value || '' });

    if (metaData.socialTitle) meta.push({ property: 'og:title', content: metaData.socialTitle.value || '' });
    if (metaData.socialDescription) meta.push({ property: 'og:description', content: metaData.socialDescription.value || '' });
    if (metaData.socialImage) meta.push({ property: 'og:image', content: metaData.socialImage.value || '' });

    return meta;
  }
}
