import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { LoginPage } from './login-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TextInputModule } from 'src/app/feature/project/reusable/inputs/fields/text-input/text-input.module';

@NgModule({
  declarations: [LoginPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: LoginPage }]),
    UbaselineCoreModule,
    ReactiveFormsModule,
    TextInputModule
  ],
  entryComponents: [LoginPage]
})
export class LoginPageModule { }
