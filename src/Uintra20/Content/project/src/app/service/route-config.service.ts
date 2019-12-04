import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Router, Route } from '@angular/router';
import { config } from '../app.config';
import { PageResolverService } from './page-resolver.service';

@Injectable({
  providedIn: 'root'
})
export class RouteConfigService {

  constructor(
    private http: HttpClient,
    private injector: Injector
  ) { }

  async resolveRoutes()
  {
    const routes = await this.http.get('/ubaseline/api/node/GetRoutes').pipe(
      map((res: any) => this.mapRoutes(res) as Route[])
    ).toPromise();

    const router = this.injector.get(Router);
    router.config = routes;
  }

  private mapRoutes(config: {url: string, contentTypeAlias: string}[])
  {
    let routes = [];
    config.forEach(el => {
      if (!this.aliasToComponent(el.contentTypeAlias)) return;

      routes.push({
        path: el.url.slice(1, el.url.length -1),
        loadChildren: this.aliasToComponent(el.contentTypeAlias),
        resolve: {data: PageResolverService}
      })
    });

    routes.push({path: "not-found", loadChildren: "./ui/pages/not-found/not-found.module#NotFoundModule"});
    routes.push({path: "**", redirectTo: "not-found"});

    return routes;
  }

  private aliasToComponent(alias: string)
  {
    const basePages = {
      contentPage: './ui/pages/content/content.module#ContentModule',
      homePage: './ui/pages/home/home.module#HomeModule',
    };

    const map = Object.assign(config.pages, basePages);

    return map[alias] || map.contentPage;
  }
}
