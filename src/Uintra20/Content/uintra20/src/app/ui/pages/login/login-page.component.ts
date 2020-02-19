import { Component, ViewEncapsulation, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ILoginPage } from './login-page.interface';

@Component({
  selector: 'login-page',
  templateUrl: './login-page.html',
  styleUrls: ['./login-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class LoginPage implements OnDestroy {
  private loginSubscription: Subscription;
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
    private router: Router) {
  }

  public ngOnDestroy(): void {
    if (this.loginSubscription != null) { this.loginSubscription.unsubscribe(); }
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
      .pipe(
        finalize(() => this.inProgress = false)
      ).subscribe(
        (next) => { this.router.navigate(['/']); },
        (error) => {
          this.errors = [];
          if (error.status === 400) {
            this.errors = error.error.message
            .split('\n')
            .filter(e => e != null && e !== '');
          }
        }
      );
  }

  private getCurrentTimeZoneId() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }
}
