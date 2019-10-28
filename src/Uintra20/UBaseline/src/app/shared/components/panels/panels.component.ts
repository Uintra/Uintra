import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-panels',
  templateUrl: './panels.component.html',
  styleUrls: ['./panels.component.less']
})
export class PanelsComponent {
  @Input() items: any[];
}
