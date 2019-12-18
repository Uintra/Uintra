import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginModel } from 'src/app/feature/login/models/login.model';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AuthService } from 'src/app/feature/auth/auth.service';

@Component({
  selector: 'login-page',
  templateUrl: './login-page.html',
  styleUrls: ['./login-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class LoginPage implements  OnDestroy {
  private loginSubscription: Subscription;
  public inProgress = false;
  public loginForm: FormGroup = new FormGroup(
    {
      login: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    }
  );

  constructor(
    private authService: AuthService,
    private router: Router) {
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

    this.authService.login(model)
      .pipe(
        finalize(() => this.inProgress = false)
      ).subscribe(
        (next) => { this.router.navigate(['/']); },
        (error) => {}
      );
  }

  private getCurrentTimeZoneId() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }
}
