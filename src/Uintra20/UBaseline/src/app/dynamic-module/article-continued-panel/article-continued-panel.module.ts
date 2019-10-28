import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArticleContinuedPanelComponent } from './article-continued-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [ArticleContinuedPanelComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  providers: [{provide: DYNAMIC_COMPONENT, useValue: ArticleContinuedPanelComponent}],
  entryComponents: [ArticleContinuedPanelComponent]
})
export class ArticleContinuedPanelModule { }
