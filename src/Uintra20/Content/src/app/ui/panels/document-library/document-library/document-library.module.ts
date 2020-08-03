import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentLibraryComponent } from './document-library.component';
import { LinkModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/link/link.module';

@NgModule({
  declarations: [DocumentLibraryComponent],
  imports: [
    CommonModule,
    LinkModule
  ],
  exports: [DocumentLibraryComponent]
})
export class DocumentLibraryModule { }
