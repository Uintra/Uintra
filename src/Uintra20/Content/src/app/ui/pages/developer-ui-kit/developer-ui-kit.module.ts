import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { DeveloperUIKitPage } from './developer-ui-kit.component';
import { FormsModule } from '@angular/forms';
import { SqDatetimepickerModule } from 'ngx-eonasdan-datetimepicker';
import { CheckboxInputModule } from 'src/app/feature/reusable/inputs/checkbox-input/checkbox-input.module';
import { TextInputModule } from 'src/app/feature/reusable/inputs/fields/text-input/text-input.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';
import { SpoilerSectionModule } from 'src/app/feature/reusable/ui-elements/spoiler-section/spoiler-section.module';
import { PublicationHeaderModule } from 'src/app/feature/reusable/ui-elements/publication-header/publication-header.module';
import { DocumentTableModule } from 'src/app/feature/specific/groups/document-table/document-table.module';

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
