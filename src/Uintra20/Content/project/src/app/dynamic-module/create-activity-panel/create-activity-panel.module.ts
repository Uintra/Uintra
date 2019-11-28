import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { CreateActivityPanelComponent } from './create-activity-panel.component';
import { CreateActivityBulletinsComponent } from './components/create-activity-bulletins/create-activity-bulletins.component';
import { CreateActivityEventsComponent } from './components/create-activity-events/create-activity-events.component';
import { CreateActivityNewsComponent } from './components/create-activity-news/create-activity-news.component';
import { PopUpBulletinComponent } from './components/pop-up-bulletin/pop-up-bulletin.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TagMultiselectComponent } from './components/create-activity-bulletins/tag-multiselect/tag-multiselect.component';
import { BulletinsTextEditorComponent } from './components/create-activity-bulletins/bulletins-text-editor/bulletins-text-editor.component';
import { QuillModule } from 'ngx-quill';

@NgModule({
  declarations: [
    CreateActivityPanelComponent,
    CreateActivityBulletinsComponent,
    CreateActivityEventsComponent,
    CreateActivityNewsComponent,
    PopUpBulletinComponent,
    TagMultiselectComponent,
    BulletinsTextEditorComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule
  ],
  providers: [ {provide: DYNAMIC_COMPONENT, useValue: CreateActivityPanelComponent}],
  entryComponents: [CreateActivityPanelComponent]
})
export class CreateActivityPanelModule { }
