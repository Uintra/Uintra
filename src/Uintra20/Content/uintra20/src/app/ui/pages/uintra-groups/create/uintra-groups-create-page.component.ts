import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { HasDataChangedService } from 'src/app/services/general/has-data-changed.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'uintra-groups-create-page',
  templateUrl: './uintra-groups-create-page.html',
  styleUrls: ['./uintra-groups-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsCreatePage {
  data: any;

  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
  ) {
    this.route.data.subscribe(data => {
      this.data = ParseHelper.parseUbaselineData(data);
      this.addButtonService.setPageId(data.id);
    });
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      if(confirm('Are you sure?')) {
        return true;
      }

      return false;
    }

    return true;
  }
}
