import { Component, ViewEncapsulation, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/shared/services/auth/auth.service';
import { ILoginPage } from './login-page.interface';
import * as moment from "moment-timezone";
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { PopUpComponent } from 'src/app/shared/ui-elements/pop-up/pop-up.component';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'login-page',
  templateUrl: './login-page.html',
  styleUrls: ['./login-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class LoginPage implements OnDestroy, OnInit {
  private $loginSubscription: Subscription;
  public inProgress = false;
  public errors = [];
  public loginForm = new FormGroup(
    {
      login: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    }
  );

  constructor(
    private authService: AuthService,
    private router: Router,
    private modalService: ModalService,
    private appService: AppService) {
  }

  ngOnInit(): void {
    this.appService.setPageAccess(true);
  }

  public ngOnDestroy(): void {
    if (this.$loginSubscription) { this.$loginSubscription.unsubscribe(); }
  }

  public submit() {
    this.inProgress = true;

    const model: ILoginPage = {
      login: this.loginForm.value.login,
      password: this.loginForm.value.password,
      clientTimeZoneId: this.getCurrentTimeZoneId(),
      returnUrl: '/'
    };

    this.authService.login(model)
      .subscribe(
        (next) => { this.router.navigate(['/']); },
        (error) => {
          this.errors = [];
          if (error.status === 400) {
            this.errors = error.error.message
            .split('\n')
            .filter(e => e != null && e !== '');
          }
          this.inProgress = false;
        }
      );
  }

  private getCurrentTimeZoneId() {
    return moment.tz.guess();
  }

  test(e) {
    e.stopPropagation();
    const testArray = [
      "1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 ",
      "11123 1123 1123 ",
      "11123 1123 1123 1123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 ",
      "11123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1123 1 ",
      "11123 1123 1123 ",
      "11123 1123 1123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 ",
      "11123 1123 1123 ",
      "11123 1123 1123 1123 1123 1123 1123 1123 1123 1123 ",
      "11123 1123 1123 1123 ",
      "11123 1123 1123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 11123 1123 1123 1123 ",
      "11123 1123 1"];
    testArray.forEach((val, index, arr) => {
      this.modalService.appendComponentToBody(PopUpComponent, {data: val}, null, index.toString(), index === arr.length - 1);
    })
  }
}
