import { Component } from '@angular/core';
import { AbstractPageComponent } from 'src/app/shared/components/abstract-page/abstract-page.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less']
})
export class HomeComponent extends AbstractPageComponent {
  defaultTitle = 'Home page';
}