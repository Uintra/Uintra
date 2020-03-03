import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { UbaselineCoreModule } from "@ubaseline/next";
import { UintraGroupsDocumentsPage } from "./uintra-groups-documents-page.component";
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { DropzoneWrapperModule } from 'src/app/feature/reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module';
import { DocumentTableModule } from 'src/app/feature/specific/groups/document-table/document-table.module';

@NgModule({
  declarations: [UintraGroupsDocumentsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: UintraGroupsDocumentsPage }]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    DropzoneWrapperModule,
    DocumentTableModule
  ],
  entryComponents: [UintraGroupsDocumentsPage]
})
export class UintraGroupsDocumentsPageModule {}
