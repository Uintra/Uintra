import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable } from 'rxjs';
import { IMyPage} from 'src/app/shared/interfaces/pages/my/my-page.interface';

@Component({
  selector: 'my-page',
  templateUrl: './my-page.html',
  styleUrls: ['./my-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class MyPage implements OnInit {

  public data: IMyPage;

  public ngOnInit(): void {   
  }

  constructor(
    private activatedRoute: ActivatedRoute,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.activatedRoute.data.subscribe((data: IMyPage) => {
      this.data = data;
    });
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  } 
}
