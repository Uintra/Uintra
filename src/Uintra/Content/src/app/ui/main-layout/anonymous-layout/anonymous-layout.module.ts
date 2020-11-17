import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AnonymousLayoutComponent } from "./anonymous-layout.component";
import { UserNavigationModule } from "src/app/feature/specific/user-navigation/user-navigation.module";
import { GoToTopButtonModule } from "src/app/feature/reusable/ui-elements/go-to-top-button/go-to-top-button.module";
import { ImageGalleryModule } from "src/app/feature/reusable/ui-elements/image-gallery/image-gallery.module";
import { HeaderModule } from '../header/header.module';
import { SearchModule } from 'src/app/feature/reusable/inputs/search/search.module';
import { LeftNavigationModule } from '../left-navigation/left-navigation.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [AnonymousLayoutComponent],
  imports: [
    CommonModule,
    UserNavigationModule,
    GoToTopButtonModule,
    ImageGalleryModule,
    HeaderModule,
    SearchModule,
    LeftNavigationModule,
    TranslateModule
  ],
  exports: [AnonymousLayoutComponent],
})
export class AnonymousLayoutModule {}
