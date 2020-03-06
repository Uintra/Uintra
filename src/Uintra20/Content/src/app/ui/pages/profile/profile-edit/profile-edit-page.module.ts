import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { ProfileEditPage } from './profile-edit-page.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { TagMultiselectModule } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { DropzoneWrapperModule } from 'src/app/feature/reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module';
import { CheckboxInputModule } from 'src/app/feature/reusable/inputs/checkbox-input/checkbox-input.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [ProfileEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ProfileEditPage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    TagMultiselectModule,
    FormsModule,
    ReactiveFormsModule,
    DropzoneWrapperModule,
    CheckboxInputModule,
    TranslateModule,
  ],
  entryComponents: [ProfileEditPage]
})
export class ProfileEditPageModule { }
