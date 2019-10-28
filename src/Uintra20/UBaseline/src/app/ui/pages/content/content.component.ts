import { Component } from '@angular/core';
import { AbstractPageComponent } from 'src/app/shared/components/abstract-page/abstract-page.component';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.less']
})
export class ContentComponent extends AbstractPageComponent {
  defaultTitle = 'Content page';
}
