import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'left-nav-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.less']
})
export class GroupsComponent implements OnInit {
  data: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get('/ubaseline/api/Group/LeftNavigation').subscribe(res => {
      this.data = res;
      console.log(res);
    })
  }

}
