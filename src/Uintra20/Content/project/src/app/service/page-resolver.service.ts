import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class PageResolverService {

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) { }

  async byPath(path: string)
  {
    const host = this.appConfigService.getHostName();

    return this.http.get(`umbraco/api/node/getByurlHomePage`).pipe(
      map(res => this.parseResponse(res))
    ).toPromise();
  }

  private parseResponse(res: any)
  {
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
