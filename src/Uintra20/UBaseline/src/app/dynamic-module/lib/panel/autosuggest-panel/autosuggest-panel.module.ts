import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutosuggestPanelComponent } from './autosuggest-panel.component';
import { AutosuggestInputComponent } from './component/autosuggest-input/autosuggest-input.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AutosuggestComponent } from './component/autosuggest/autosuggest.component';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/service/translations-loader';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { AutosuggestResultsComponent } from './component/autosuggest-results/autosuggest-results.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [AutosuggestPanelComponent, AutosuggestInputComponent, AutosuggestComponent, AutosuggestResultsComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateModule, 
        deps: [HttpClient],
        useClass: TranslationsLoader},
    })
  ],
  providers: [
    {provide: DYNAMIC_COMPONENT, useValue: AutosuggestPanelComponent},
  ],
  entryComponents: [AutosuggestPanelComponent, AutosuggestComponent],
  exports: [AutosuggestInputComponent, AutosuggestComponent]
})
export class AutosuggestPanelModule {}
