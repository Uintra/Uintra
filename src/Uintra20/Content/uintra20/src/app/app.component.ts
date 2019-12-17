import { Component, OnInit } from '@angular/core';
import { LoginService } from './feature/login/services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  title = 'uintra20';
  isLogged = false;

  constructor(
    private loginService: LoginService
  ) { }

  ngOnInit(): void {
     this.loginService.getState().subscribe(state => {
      this.isLogged = state;
    });

  }
}
