import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { LoginPage } from './login-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { PopUpModule } from 'src/app/shared/ui-elements/pop-up/pop-up.module';
import { PopUpComponent } from 'src/app/shared/ui-elements/pop-up/pop-up.component';
import { ModalService } from 'src/app/shared/services/general/modal.service';

@NgModule({
  declarations: [LoginPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: LoginPage }]),
    UbaselineCoreModule,
    ReactiveFormsModule,
    TranslateModule,
    PopUpModule
  ],
  providers: [ModalService],
  entryComponents: [PopUpComponent]
})
export class LoginPageModule { }
