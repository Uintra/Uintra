import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { UbaselineCoreModule } from "@ubaseline/next";
import { UintraNewsEditPage } from "./uintra-news-edit-page.component";
import { DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { DEFAULT_DROPZONE_CONFIG } from 'src/app/shared/constants/dropzone/drop-zone.const';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { NewsFormModule } from 'src/app/feature/specific/activity/news-form/news-form.module';
import { RichTextEditorModule } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';

@NgModule({
  declarations: [UintraNewsEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: UintraNewsEditPage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    NewsFormModule,
    UlinkModule,
    RichTextEditorModule.configure({
      modules: {
        counter: {
          maxLength: MAX_LENGTH
        }
      }
    }),
    GroupDetailsWrapperModule,
  ],
  entryComponents: [UintraNewsEditPage],
  providers: [{
    provide: DROPZONE_CONFIG,
    useValue: DEFAULT_DROPZONE_CONFIG
  }]
})
export class UintraNewsEditPageModule {}
