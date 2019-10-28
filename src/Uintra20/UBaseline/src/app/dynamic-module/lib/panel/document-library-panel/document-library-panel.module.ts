import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentLibraryPanelComponent } from './document-library-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { DocumentLibraryModule } from './document-library/document-library.module';


@NgModule({
  declarations: [DocumentLibraryPanelComponent],
  imports: [
    CommonModule,
    DocumentLibraryModule
  ],
  providers: [{provide: DYNAMIC_COMPONENT, useValue: DocumentLibraryPanelComponent}],
  exports: [DocumentLibraryPanelComponent],
  entryComponents: [
    DocumentLibraryPanelComponent
  ]
})
export class DocumentLibraryPanelModule { }
