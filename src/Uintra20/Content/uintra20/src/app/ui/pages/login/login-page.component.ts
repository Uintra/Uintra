import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginService } from 'src/app/feature/login/services/login.service';
import { LoginModel } from 'src/app/feature/login/models/login.model';
import { Subscription } from 'rxjs';
import { filter, finalize } from 'rxjs/operators';

@Component({
  selector: 'login-page',
  templateUrl: './login-page.html',
  styleUrls: ['./login-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class LoginPage implements OnInit, OnDestroy {
  private loginSubscription: Subscription;
  public inProgress = false;
  public loginForm: FormGroup = new FormGroup(
    {
      login: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    }
  );

  constructor(
    private loginService: LoginService,
    private router: Router) {
  }

  public ngOnInit(): void {
    this.loginService.getState()
      .pipe(
        finalize(() => this.inProgress = false)).subscribe(
          (next) => {
            this.inProgress = false;
            if (next !== null) {
              this.router.navigate(['/']);
            } else {
              this.router.navigate(['/login'])
            }
          },
          (error) => { console.log(error); },
          () => { }
        );
  }

  public ngOnDestroy(): void {
    if (this.loginSubscription != null) { this.loginSubscription.unsubscribe(); }
  }

  public submit() {
    this.inProgress = true;

    const model = new LoginModel(
      this.loginForm.value.login,
      this.loginForm.value.password,
      this.getCurrentTimeZoneId(),
      '/'
    );

    this.loginService.login(model).then(
      () => {},
      () => {this.inProgress = false; }
    );
  }

  private getCurrentTimeZoneId() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }
}
