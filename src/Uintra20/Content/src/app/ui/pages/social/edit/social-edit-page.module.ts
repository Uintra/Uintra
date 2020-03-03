import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { UbaselineCoreModule, AS_DYNAMIC_COMPONENT } from "@ubaseline/next";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SocialEditPageComponent } from "./social-edit-page.component";
import { HttpClientModule } from "@angular/common/http";
import { CanDeactivateGuard } from "src/app/shared/services/general/can-deactivate.service";
import { MAX_LENGTH } from "src/app/shared/constants/activity/activity-create.const";
import { TagMultiselectModule } from "src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.module";
import { DropzoneWrapperModule } from "src/app/feature/reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module";
import { GroupDetailsWrapperModule } from "src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module";
import { RichTextEditorModule } from "src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.module";
import { DropzoneExistingImagesModule } from 'src/app/feature/reusable/ui-elements/dropzone-existing-images/dropzone-existing-images.module';

@NgModule({
  declarations: [SocialEditPageComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: "",
        component: SocialEditPageComponent,
        canDeactivate: [CanDeactivateGuard]
      }
    ]),
    UbaselineCoreModule,
    TagMultiselectModule,
    FormsModule,
    ReactiveFormsModule,
    DropzoneWrapperModule,
    HttpClientModule,
    GroupDetailsWrapperModule,
    DropzoneExistingImagesModule,
    RichTextEditorModule.configure({
      modules: {
        counter: {
          maxLength: MAX_LENGTH
        }
      }
    })
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: SocialEditPageComponent }
  ],
  entryComponents: [SocialEditPageComponent]
})
export class SocialEditPageModule {}
