import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { LoginPage } from './login-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [LoginPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: LoginPage }]),
    UbaselineCoreModule,
    ReactiveFormsModule,
    TranslateModule,
  ],
  entryComponents: [LoginPage]
})
export class LoginPageModule { }
