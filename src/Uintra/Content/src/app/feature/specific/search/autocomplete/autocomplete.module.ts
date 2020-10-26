import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutocompleteComponent } from './autocomplete.component';
import { FormsModule } from '@angular/forms';
import { TextInputModule } from 'src/app/feature/reusable/inputs/fields/text-input/text-input.module';
import { TranslateModule } from '@ngx-translate/core';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { RouterModule } from '@angular/router';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';
import { UserAvatarModule } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar.module';



@NgModule({
  declarations: [AutocompleteComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    TextInputModule,
    TranslateModule,
    UlinkModule,
    ClickOutsideModule,
    UserAvatarModule,
  ],
  exports: [AutocompleteComponent]
})
export class AutocompleteModule { }
