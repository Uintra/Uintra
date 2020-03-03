import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { LoginPage } from './login-page.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [LoginPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: LoginPage }]),
    UbaselineCoreModule,
    ReactiveFormsModule,
  ],
  entryComponents: [LoginPage]
})
export class LoginPageModule { }
