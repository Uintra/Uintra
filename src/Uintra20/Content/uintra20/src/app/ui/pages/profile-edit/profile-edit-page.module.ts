import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { ProfileEditPage } from './profile-edit-page.component';
import { TagMultiselectModule } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DropzoneModule, DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { DEFAULT_DROPZONE_CONFIG } from 'src/app/constants/dropzone/drop-zone.const';

@NgModule({
  declarations: [ProfileEditPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ProfileEditPage }]),
    UbaselineCoreModule,
    TagMultiselectModule,
    FormsModule,
    ReactiveFormsModule,
    DropzoneModule,
  ],
  providers: [
    {
      provide: DROPZONE_CONFIG,
      useValue: DEFAULT_DROPZONE_CONFIG
    }
  ],
  entryComponents: [ProfileEditPage]
})
export class ProfileEditPageModule { }
