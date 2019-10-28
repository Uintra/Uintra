import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { ISubscribeConfig, SUBSCRIBE_MODULE_CONFIG } from './config';
import { ModuleWithProviders } from '@angular/compiler/src/core';
import { SubscribeComponent } from './component/subscribe/subscribe.component';
import { ButtonComponent } from './component/button/button.component';
import { SubscribeService } from './service/subscribe.service';
import { LinkModule  } from '../../../ui-kit/link/link.module';
import { ButtonModule } from '../../../ui-kit/button/button.module';
import { CheckboxModule } from '../../../ui-kit/checkbox/checkbox.module';

@NgModule({
  declarations: [
    SubscribeComponent, ButtonComponent
  ],
  imports: [
    CommonModule,
    TranslateModule,
    FormsModule,
    LinkModule,
    ButtonModule,
    CheckboxModule
  ],
  exports: [
    SubscribeComponent
  ]
})
export class SubscribeModule {
  static configure(config: ISubscribeConfig): ModuleWithProviders
  {
    return {
      ngModule: SubscribeModule,
      providers: [
        {
          provide: SUBSCRIBE_MODULE_CONFIG,
          useValue: config
        },
        SubscribeService
      ]
    }
  }
}
