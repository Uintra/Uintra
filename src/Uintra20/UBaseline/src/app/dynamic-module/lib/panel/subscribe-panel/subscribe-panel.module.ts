import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubscribePanelComponent } from './subscribe-panel.component';
import { DYNAMIC_COMPONENT } from 'src/app/shared/dynamic-component-loader/dynamic-component.manifest';
import { SubscribeModule } from './subscribe/subscribe.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [SubscribePanelComponent],
  imports: [
    CommonModule,
    SubscribeModule.configure({'api': '/ubaseline/api', 'agreementsUrl': '/agreements/url'}),
    SharedModule
  ],
  providers: [
    {provide: DYNAMIC_COMPONENT, useValue: SubscribePanelComponent},
  ],
  entryComponents: [SubscribePanelComponent],
  exports: [SubscribePanelComponent]
})
export class SubscribePanelModule {}
