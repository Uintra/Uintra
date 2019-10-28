import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER, ErrorHandler } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouteConfigService } from './service/route-config.service';
import { HeaderComponent } from './ui/main-layout-components/header/header.component';
import { FooterComponent } from './ui/main-layout-components/footer/footer.component';
import { MainComponent } from './ui/main-layout-components/main/main.component';
import { DynamicComponentLoaderModule } from './shared/dynamic-component-loader/dynamic-component-loader.module';
import { dynamicComponents } from './ui/dynamic-component-manifest';
import { TopLevelNavigationComponent } from './ui/main-layout-components/top-level-navigation/top-level-navigation.component';
import { components } from './project-specific-component';
import { SharedModule } from './shared/shared.module';
import { CookieModule } from './dynamic-module/cookie/cookie.module';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslationsLoader } from './service/translations-loader';
import { AppErrorHandler } from './app-error-handler';
import { AUTOSUGGEST_CONFIG } from './dynamic-module/lib/panel/autosuggest-panel/config';
import { config } from './app.config';
import { DrawerModule } from './dynamic-module/lib/drawer/drawer.module';

export function initSettings(settings: RouteConfigService) {
  return () => settings.resolveRoutes();
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    MainComponent,
    TopLevelNavigationComponent,
    ...components,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    DynamicComponentLoaderModule.forRoot(dynamicComponents),
    SharedModule,
    CookieModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: TranslationsLoader,
        deps: [HttpClient]
      }
    }),
    DrawerModule.forRoot()
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initSettings,
      deps: [RouteConfigService],
      multi: true
    },
    {
      provide: ErrorHandler,
      useClass: AppErrorHandler
    },
    {
      provide: AUTOSUGGEST_CONFIG,
      useValue: {endPoint: config.autoSuggestApi, page: 0, searchPageUrl: 'search-result-page'}
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
