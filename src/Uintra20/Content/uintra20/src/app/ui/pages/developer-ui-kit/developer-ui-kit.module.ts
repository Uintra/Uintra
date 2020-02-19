import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { DeveloperUIKitPage } from './developer-ui-kit.component';
import { TextInputModule } from 'src/app/feature/project/reusable/inputs/fields/text-input/text-input.module';
import { FormsModule } from '@angular/forms';
import { CheckboxInputModule } from 'src/app/feature/project/reusable/inputs/checkbox-input/checkbox-input.module';
import { UserAvatarModule } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module';
import { SpoilerSectionModule } from 'src/app/feature/project/reusable/ui-elements/spoiler-section/spoiler-section.module';
import { PublicationHeaderModule } from 'src/app/feature/project/reusable/ui-elements/publication-header/publication-header.module';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { DocumentTableModule } from 'src/app/feature/project/specific/groups/document-table/document-table.module';

@NgModule({
  declarations: [DeveloperUIKitPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: DeveloperUIKitPage}]),
    UbaselineCoreModule,
    FormsModule,
    TextInputModule,
    CheckboxInputModule,
    UserAvatarModule,
    SpoilerSectionModule,
    PublicationHeaderModule,
    SqDatetimepickerModule,
    DocumentTableModule
  ],
  exports: [
    DeveloperUIKitPage
  ],
  entryComponents: [DeveloperUIKitPage]
})
export class DeveloperUIKitModule {}
