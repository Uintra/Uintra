import { Component } from '@angular/core';
import { AbstractPageComponent } from 'src/app/shared/components/abstract-page/abstract-page.component';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.less']
})
export class NotFoundComponent extends AbstractPageComponent {
  defaultTitle = "Not found";
}
