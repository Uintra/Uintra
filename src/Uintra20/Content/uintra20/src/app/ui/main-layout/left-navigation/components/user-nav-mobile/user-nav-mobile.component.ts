import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IMobileUserNavigation } from '../../left-navigation.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-nav-mobile',
  templateUrl: './user-nav-mobile.component.html',
  styleUrls: ['./user-nav-mobile.component.less']
})
export class UserNavMobileComponent implements OnInit {
  data: IMobileUserNavigation;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.http.get('ubaseline/api/intranetNavigation/mobileNavigation').subscribe(
      (res: any) => {
        this.data = res;
        console.log(this.data)
      } 
    );
  }

  handleRequest(type, url) {
    if (type == 4) {
      this.http.post(url, null).subscribe(res => this.router.navigate(['/login']));
    }
  }
}
