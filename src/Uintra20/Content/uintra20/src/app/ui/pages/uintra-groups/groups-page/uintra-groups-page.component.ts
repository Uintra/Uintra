import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { GroupsService } from 'src/app/feature/project/specific/groups/groups.service';

@Component({
  selector: 'uintra-groups-page',
  templateUrl: './uintra-groups-page.html',
  styleUrls: ['./uintra-groups-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsPage implements OnInit {

  private data: any;

  constructor(
    private route: ActivatedRoute,
  ) {
    this.route.data.subscribe(data => {
      this.data = ParseHelper.parseUbaselineData(data);
      console.log(this.data);
    });
  }

  public ngOnInit(): void {
  }
}
