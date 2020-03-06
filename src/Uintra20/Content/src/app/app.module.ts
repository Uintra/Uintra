import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

import { DYNAMIC_COMPONENTS, UmbracoSupportModule, UbaselineCoreModule } from '@ubaseline/next';
import { pages } from './ui/pages/pages';
import { panels } from './ui/panels/panels';
import { LeftNavigationModule } from './ui/main-layout/left-navigation/left-navigation.module';
import { HeaderModule } from './ui/main-layout/header/header.module';

import {UrlSerializer} from '@angular/router';
import { CustomUrlSerializer } from './shared/utils/CustomUrlSerializer';
import { CanDeactivateGuard } from './shared/services/general/can-deactivate.service';
import { HasDataChangedService } from './shared/services/general/has-data-changed.service';
import { UserNavigationModule } from './feature/specific/user-navigation/user-navigation.module';
import { GoToTopButtonModule } from './feature/reusable/ui-elements/go-to-top-button/go-to-top-button.module';
import { ImageGalleryModule } from './feature/reusable/ui-elements/image-gallery/image-gallery.module';
import { NavNotificationsModule } from './feature/specific/nav-notifications/nav-notifications.module';
import { SearchModule } from './feature/reusable/inputs/search/search.module';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslationsLoader } from './shared/services/general/translations-loader';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    HeaderModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    UserNavigationModule,
    GoToTopButtonModule,
    ImageGalleryModule,
    UmbracoSupportModule.configure({
      apiPrefix: '/ubaseline/api',
      pages: pages,
      environment: environment
    }),
    NavNotificationsModule,
    LeftNavigationModule,
    SearchModule,
    UbaselineCoreModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: TranslationsLoader,
      }
    }),
  ],
  providers: [
    { provide: DYNAMIC_COMPONENTS, useValue: panels },
    { provide: UrlSerializer, useClass: CustomUrlSerializer },
    HasDataChangedService,
    CanDeactivateGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
