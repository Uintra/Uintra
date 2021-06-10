import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, ActivationStart, ChildActivationStart, ChildActivationEnd } from '@angular/router';
import { LoginPage } from './ui/pages/login/login-page.component';
import { TranslateService } from '@ngx-translate/core';
import { IApplication } from './shared/interfaces/components/application/iapplication.interface';
import { HeaderService } from './shared/services/general/header.service';
import { ViewportScroller } from '@angular/common';
import { ResolveService } from 'ubaseline-next-for-uintra';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  alive$ = new Subject();

  public title = 'uintra20';
  public isLoginPage = true;
  public hasLeftLoginPage = true;
  public hasPanels = false;

  public data: IApplication;
  public latestActivities: any;
  constructor(
    private router: Router,
    private translateService: TranslateService,
    private viewportScroller: ViewportScroller,
    private resolveService: ResolveService,
  ) {
    this.resolveService.pageDataChanged$.pipe(takeUntil(this.alive$)).subscribe((data: IApplication) => {
      this.data = data;
      this.hasPanels = data && data.panels && data.panels.length;
    });

    this.router.events.subscribe(val => {
      if (val instanceof ActivationStart) {
        if (val.snapshot.component) {
          this.isLoginPage = val.snapshot.component === LoginPage;
          if (this.isLoginPage) {
            this.hasLeftLoginPage = false;
            document.title = 'Login | Uintra';
          }
        }
      }
      if (!this.isLoginPage && val instanceof ChildActivationEnd) {
        this.hasLeftLoginPage = true;
      }
    });
  }

  public ngOnInit(): void {
    this.translateService.use('');
  }

  public closeLeftNav(): void {
    document.body.classList.remove('nav--open');

  }

  scrollToBlock(event) {
    event.preventDefault();
    let targetElement = document.querySelectorAll(event.currentTarget.getAttribute('href'))[0];


    targetElement.focus();
    targetElement.blur();

    this.viewportScroller.scrollToPosition([0, targetElement.offsetTop])
  }

  ngOnDestroy(): void {
    this.alive$.next();
    this.alive$.complete();
  }
}

