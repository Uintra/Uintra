import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { CentralFeedPanel } from './central-feed-panel.component';
import { SpoilerSectionModule } from 'src/app/feature/project/reusable/ui-elements/spoiler-section/spoiler-section.module';
import { RadioLinkGroupModule } from 'src/app/feature/project/reusable/inputs/radio-link-group/radio-link-group.module';
import { CheckboxInputModule } from 'src/app/feature/project/reusable/inputs/checkbox-input/checkbox-input.module';
import { UserAvatarModule } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module';
import { PublicationHeaderModule } from 'src/app/feature/project/reusable/ui-elements/publication-header/publication-header.module';
import { CentralFeedPublicationComponent } from './central-feed-publication/central-feed-publication.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { CentralFeedFiltersComponent } from './central-feed-filters/central-feed-filters.component';
import { LikeButtonModule } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.module';
import { RouterModule } from '@angular/router';
import { TruncatePipe } from 'src/app/services/pipes/limit-to.pipe';
import { UlinkModule } from 'src/app/services/pipes/link/ulink.module';

@NgModule({
  declarations: [CentralFeedPanel, CentralFeedPublicationComponent, CentralFeedFiltersComponent, TruncatePipe],
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
    UlinkModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: CentralFeedPanel}],
  entryComponents: [CentralFeedPanel]
})
export class CentralFeedPanelModule { }
