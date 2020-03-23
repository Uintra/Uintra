import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { SearchPage } from './search-page.component';
import { TranslateModule } from '@ngx-translate/core';
import { TagMultiselectModule } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.module';
import { FormsModule } from '@angular/forms';
import { CheckboxInputModule } from 'src/app/feature/reusable/inputs/checkbox-input/checkbox-input.module';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [SearchPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: SearchPage}]),
    UbaselineCoreModule,
    TranslateModule,
    FormsModule,
    TagMultiselectModule,
    CheckboxInputModule,
    UlinkModule,
    InfiniteScrollModule,
  ],
  entryComponents: [SearchPage]
})
export class SearchPageModule {}
