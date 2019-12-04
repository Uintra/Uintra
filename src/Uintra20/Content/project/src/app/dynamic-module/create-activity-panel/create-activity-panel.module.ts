import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { CreateActivityPanelComponent } from './create-activity-panel.component';
import { CreateActivityEventsComponent } from './components/create-activity-events/create-activity-events.component';
import { CreateActivityNewsComponent } from './components/create-activity-news/create-activity-news.component';
import { PopUpBulletinComponent } from './components/pop-up-bulletin/pop-up-bulletin.component';
import { CreateActivityBulletinsModule } from './components/create-activity-bulletins/create-activity-bulletins.module';

@NgModule({
  declarations: [
    CreateActivityPanelComponent,
    CreateActivityEventsComponent,
    CreateActivityNewsComponent,
    PopUpBulletinComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    CreateActivityBulletinsModule
  ],
  providers: [
    { provide: DYNAMIC_COMPONENT, useValue: CreateActivityPanelComponent },
  ],
  entryComponents: [CreateActivityPanelComponent]
})
export class CreateActivityPanelModule {}
