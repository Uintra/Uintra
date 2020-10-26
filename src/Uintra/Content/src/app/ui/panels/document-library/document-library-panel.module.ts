import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { DocumentLibraryPanelComponent } from './document-library-panel.component';
import { DocumentLibraryModule } from './document-library/document-library.module';

@NgModule({
  declarations: [DocumentLibraryPanelComponent],
  imports: [
    CommonModule,
    DocumentLibraryModule
  ],
  providers: [{ provide: AS_DYNAMIC_COMPONENT, useValue: DocumentLibraryPanelComponent }],
  exports: [DocumentLibraryPanelComponent],
  entryComponents: [DocumentLibraryPanelComponent]
})
export class DocumentLibraryPanelModule {}
