import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PictureComponent } from './components/picture/picture.component';
import { PanelsComponent } from './components/panels/panels.component';
import { ButtonComponent } from './components/button/button.component';
import { IframeComponent } from './components/iframe/iframe.component';
import { DynamicComponent } from './components/dynamic/dynamic.component';
import { AbstractPageComponent } from './components/abstract-page/abstract-page.component';
import { RouterModule } from '@angular/router';
import { ClickOutsideDirective } from './directive/click-outside.directive';

import { specificComponents } from './shared-specific-component';
import { specificImports } from './shared-specific-imports';
import { HeadingComponent } from './components/heading/heading.component';
import { AutosuggestInputComponent } from './components/autosuggest-input/autosuggest-input.component';

import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/service/translations-loader';
import { ModalComponent } from './components/modal/modal.component';
import { ModalVideoComponent } from './components/modal-video/modal-video.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FieldsModule } from './components/fields/fields.module';

const components = [
  TextInputComponent,
  PictureComponent,
  PanelsComponent,
  IframeComponent,
  ButtonComponent,
  DynamicComponent,
  AbstractPageComponent,
  ...specificComponents,
  ClickOutsideDirective,
  HeadingComponent,
  AutosuggestInputComponent,
  ModalComponent,
  ModalVideoComponent,
];

@NgModule({
  declarations: components,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    FieldsModule,
    ReactiveFormsModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateModule,
        deps: [HttpClient],
        useClass: TranslationsLoader},
    }),
    ...specificImports
  ],
  exports: [
    ...components, FieldsModule
  ]
})
export class SharedModule { }
