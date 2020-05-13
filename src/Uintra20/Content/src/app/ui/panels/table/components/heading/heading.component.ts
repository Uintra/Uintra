import { Component, Input, HostBinding } from '@angular/core';

@Component({
  selector: 'app-heading',
  templateUrl: './heading.component.html',
  styleUrls: ['./heading.component.less']
})

export class HeadingComponent {
  @HostBinding('class') get hostClassName() {return 'heading'}
  @Input() level: any;
  @Input() note: string;
}
