import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { ProfileEditPage } from './profile-edit-page.component';
import { TagMultiselectModule } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DropzoneWrapperModule } from 'src/app/feature/project/reusable/ui-elements/dropzone-wrapper/dropzone-wrapper.module';
import { CheckboxInputModule } from 'src/app/feature/project/reusable/inputs/checkbox-input/checkbox-input.module';

@NgModule({
  declarations: [ProfileEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ProfileEditPage }]),
    UbaselineCoreModule,
    TagMultiselectModule,
    FormsModule,
    ReactiveFormsModule,
    DropzoneWrapperModule,
    CheckboxInputModule
  ],
  entryComponents: [ProfileEditPage]
})
export class ProfileEditPageModule { }
