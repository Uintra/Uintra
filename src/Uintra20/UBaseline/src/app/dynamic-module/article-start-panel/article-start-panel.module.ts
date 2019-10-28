import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArticleStartPanelComponent } from './article-start-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';

@NgModule({
  declarations: [ArticleStartPanelComponent],
  imports: [
    CommonModule
  ],
  providers: [{provide: DYNAMIC_COMPONENT, useValue: ArticleStartPanelComponent}],
  entryComponents: [ArticleStartPanelComponent]
})
export class ArticleStartPanelModule { }
