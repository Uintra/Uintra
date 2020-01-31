import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { UbaselineCoreModule } from "@ubaseline/next";
import { UintraNewsEditPage } from "./uintra-news-edit-page.component";
import { NewsFormModule } from "src/app/feature/project/specific/activity/news-form/news-form.module";
import { RichTextEditorModule } from "src/app/feature/project/reusable/inputs/rich-text-editor/rich-text-editor.module";
import { MAX_LENGTH } from "src/app/constants/activity/create/activity-create-const";
import { DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { DEFAULT_DROPZONE_CONFIG } from 'src/app/constants/dropzone/drop-zone.const';

@NgModule({
  declarations: [UintraNewsEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: UintraNewsEditPage }]),
    UbaselineCoreModule,
    NewsFormModule,
    RichTextEditorModule.configure({
      modules: {
        counter: {
          maxLength: MAX_LENGTH
        }
      }
    })
  ],
  entryComponents: [UintraNewsEditPage],
  providers: [{
    provide: DROPZONE_CONFIG,
    useValue: DEFAULT_DROPZONE_CONFIG
  }]
})
export class UintraNewsEditPageModule {}
