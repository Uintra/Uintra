import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginService } from 'src/app/feature/login/services/login.service';
import { LoginModel } from 'src/app/feature/login/models/login.model';
import { finalize } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'login-page',
  templateUrl: './login-page.html',
  styleUrls: ['./login-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class LoginPage implements OnInit, OnDestroy {
  private inProgress = false;
  private loginSubscription: Subscription;
  private loginForm: FormGroup = new FormGroup(
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
  }

  public ngOnDestroy(): void {
    if (this.loginSubscription != null) { this.loginSubscription.unsubscribe(); }
  }

  private submit() {
    const model = new LoginModel(
      this.loginForm.value.login,
      this.loginForm.value.password,
      this.getCurrentTimeZoneId(),
      '/'
    );
    this.inProgress = true;
    this.loginSubscription = this.loginService.login(model)
      .pipe(
        finalize(() => setTimeout(() => this.inProgress = false, 400))
      ).subscribe(
        (next) => this.router.navigate([next.redirectUrl]),
        (error) => { console.log(error); },
        () => { }
      );
  }

  private getCurrentTimeZoneId() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }
}
