import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutosuggestPanelComponent } from './autosuggest-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { AutosuggestResultsComponent } from './component/autosuggest-results/autosuggest-results.component';
import { AutosuggestComponent } from './component/autosuggest/autosuggest.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/service/translations-loader';

@NgModule({
  declarations: [AutosuggestPanelComponent, AutosuggestResultsComponent, AutosuggestComponent],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    TranslateModule
  ],
  providers: [{provide: DYNAMIC_COMPONENT, useValue: AutosuggestPanelComponent}],
  exports: [AutosuggestComponent, AutosuggestPanelComponent],
  entryComponents: [AutosuggestPanelComponent]
})
export class AutosuggestPanelModule { }
