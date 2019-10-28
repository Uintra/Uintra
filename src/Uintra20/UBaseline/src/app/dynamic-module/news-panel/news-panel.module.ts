import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsPanelComponent } from './news-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';
import { RouterModule } from '@angular/router';
import { NewsPanelItemComponent } from './component/news-panel-item/news-panel-item.component';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/service/translations-loader';

@NgModule({
  declarations: [NewsPanelComponent, NewsPanelItemComponent],
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
  providers: [ {provide: DYNAMIC_COMPONENT, useValue: NewsPanelComponent}],
  entryComponents: [NewsPanelComponent]
})
export class NewsPanelModule { }
