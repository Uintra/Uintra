import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { LikesPanel } from './likes-panel.component';
import { LikeButtonModule } from 'src/app/feature/reusable/ui-elements/like-button/like-button.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [LikesPanel],
  imports: [
    CommonModule,
    NotImplementedModule,
    LikeButtonModule,
    TranslateModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: LikesPanel}],
  entryComponents: [LikesPanel]
})
export class LikesPanelModule {}
