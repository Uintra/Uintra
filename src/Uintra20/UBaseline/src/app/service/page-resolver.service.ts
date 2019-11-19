import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { AppConfigService } from './app-config.service';
import { ActivatedRouteSnapshot } from '@angular/router';
import { IPageData } from '../shared/components/abstract-page/abstract-page.component';

@Injectable({
  providedIn: 'root'
})
export class PageResolverService {
  private cache = new Map<string, IPageData>();

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) { }

  async resolve(route: ActivatedRouteSnapshot) {
    const path = `/${route.url.join('/')}`;
    const cached = this.cache.get(path);

    if (!cached) {
      const data = await this.byPath(path);
      this.cache.set(path, data);
    }

    return this.cache.get(path);
  }

  async byPath(path: string) {
    const previewQuery = this.tryGetPreviewQuery();
    const currentHost = this.appConfigService.getHostName();
    const host = await this.getHost(currentHost);

    if (currentHost === host) {
      return this.http.get(`ubaseline/api/node/getByurl?url=${host}${path}${previewQuery}`).pipe(
        map(res => this.parseResponse(res))
      ).toPromise();
    }
    else {
      window.location.href = host + path + previewQuery;
    }
  }

  async getHost(currentHost : string) {
    const culture = this.tryGetCulture();
    if (culture) {
      return this.http.get(`ubaseline/api/domain/getDomainByCulture?culture=${culture}`).pipe(
        map(res => { return res; })
      ).toPromise();
    }
    else {
      return currentHost;
    }
  }
  private tryGetPreviewQuery() {
    return window.location.search.indexOf("preview") === -1 ? "" : window.location.search;
  }

  private tryGetCulture() {
    const cultureHash = "#?culture=";
    return window.location.search.indexOf("preview") > -1 && window.location.hash.startsWith(cultureHash) ?
      window.location.hash.split(cultureHash)[1] :
      "";
  }

  private parseResponse(res: any) {
    if (res === null) return res;
    if (!res.isDefaultModel) return res;

    let data = {};

    res['properties'].forEach(prop => {
      data[prop['alias']] = {
        value: prop['value'],
        editorAlias: prop['editorAlias']
      }
    });

    return data;
  }
}
