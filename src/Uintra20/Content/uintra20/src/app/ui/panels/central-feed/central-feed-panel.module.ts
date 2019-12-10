import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { CentralFeedPanel } from './central-feed-panel.component';
import { SpoilerSectionModule } from 'src/app/feature/project/reusable/ui-elements/spoiler-section/spoiler-section.module';
import { RadioLinkGroupModule } from 'src/app/feature/project/reusable/inputs/radio-link-group/radio-link-group.module';
import { CheckboxInputModule } from 'src/app/feature/project/reusable/inputs/checkbox-input/checkbox-input.module';
import { UserAvatarModule } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar.module';

@NgModule({
  declarations: [CentralFeedPanel],
  imports: [
    CommonModule,
    NotImplementedModule,
    SpoilerSectionModule,
    RadioLinkGroupModule,
    CheckboxInputModule,
    FormsModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: CentralFeedPanel}],
  entryComponents: [CentralFeedPanel]
})
export class CentralFeedPanelModule {}
