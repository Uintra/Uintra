import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsCreatePage } from './uintra-groups-create-page.component';
import { RichTextEditorModule } from 'src/app/feature/project/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { TextInputModule } from 'src/app/feature/project/reusable/inputs/fields/text-input/text-input.module';
import { DropzoneWrapperModule } from 'src/app/feature/project/reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [UintraGroupsCreatePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraGroupsCreatePage}]),
    UbaselineCoreModule,
    TextInputModule,
    RichTextEditorModule,
    FormsModule,
    DropzoneWrapperModule,
  ],
  entryComponents: [UintraGroupsCreatePage]
})
export class UintraGroupsCreatePageModule {}
