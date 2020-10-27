import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventSubscriptionComponent } from './event-subscription.component';
import { CheckboxInputModule } from 'src/app/feature/reusable/inputs/checkbox-input/checkbox-input.module';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [EventSubscriptionComponent],
  imports: [
    CommonModule,
    FormsModule,
    CheckboxInputModule,
    TranslateModule,
  ],
  exports: [EventSubscriptionComponent]
})
export class EventSubscriptionModule { }
