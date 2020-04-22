import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from "@ubaseline/next";
import { CentralFeedPanel } from "./central-feed-panel.component";
import { CentralFeedPublicationComponent } from "./central-feed-publication/central-feed-publication.component";
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { CentralFeedFiltersComponent } from "./central-feed-filters/central-feed-filters.component";
import { RouterModule } from "@angular/router";
import { UlinkModule } from "src/app/shared/pipes/link/ulink.module";
import { SpoilerSectionModule } from "src/app/feature/reusable/ui-elements/spoiler-section/spoiler-section.module";
import { RadioLinkGroupModule } from "src/app/feature/reusable/inputs/radio-link-group/radio-link-group.module";
import { CheckboxInputModule } from "src/app/feature/reusable/inputs/checkbox-input/checkbox-input.module";
import { PublicationHeaderModule } from "src/app/feature/reusable/ui-elements/publication-header/publication-header.module";
import { LikeButtonModule } from "src/app/feature/reusable/ui-elements/like-button/like-button.module";
import { TranslateModule } from '@ngx-translate/core';
import { LinkPreviewModule } from 'src/app/feature/specific/link-preview/link-preview.module';

@NgModule({
  declarations: [
    CentralFeedPanel,
    CentralFeedPublicationComponent,
    CentralFeedFiltersComponent
  ],
  imports: [
    CommonModule,
    NotImplementedModule,
    SpoilerSectionModule,
    RadioLinkGroupModule,
    CheckboxInputModule,
    PublicationHeaderModule,
    FormsModule,
    InfiniteScrollModule,
    LikeButtonModule,
    RouterModule,
    UlinkModule,
    TranslateModule,
    LinkPreviewModule,
  ],
  providers: [{ provide: AS_DYNAMIC_COMPONENT, useValue: CentralFeedPanel }],
  entryComponents: [CentralFeedPanel]
})
export class CentralFeedPanelModule {}
