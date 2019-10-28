import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { config } from '../app.config';
import { map } from 'rxjs/operators';
import { IPictureData } from '../shared/components/picture/picture.component';
import { Observable } from 'rxjs';
import { ISiteSettings } from '../shared/interface/site-settings';


export interface ISiteSettingsResponse {
  headerSubLinks?: any;
  footerLogo?: IPictureData;
  footerItems?: any;
  siteTitle?: string;
  siteLogo?: IPictureData;
  cookieTitle?: string;
  cookieDescription?: string;
  culture: string;
  pageTitleSeparator: string;
}
@Injectable({
  providedIn: 'root'
})
export class SiteSettingsService {
  private cache: ISiteSettings;

  constructor(
    private http: HttpClient
  ) { }

  async getSiteSettings()
  {
   if (!this.cache)
   {
     this.cache = await this.loadSettings().toPromise();

     return this.cache;
   }

   return this.cache;
  }

  private loadSettings()
  {
    return this.http.get<ISiteSettingsResponse>(`${config.api}/siteSettings/get`).pipe(
      map(response => {
        return {
          culture: response.culture,
          pageTitleSeparator: response.pageTitleSeparator,
          headerSettings: {
            headerSubLinks: response.headerSubLinks,
            siteTitle: response.siteTitle,
            siteLogo: response.siteLogo
          },
          footerSettings: {
            footerLogo: response.footerLogo,
            footerItems: response.footerItems
          },
          cookieSettings: {
            cookieTitle: response.cookieTitle,
            cookieDescription: response.cookieDescription
          }
        }
      })
    )
  }
}