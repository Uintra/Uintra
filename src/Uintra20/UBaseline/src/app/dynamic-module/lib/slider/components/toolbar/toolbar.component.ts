import { Component, Input, Output, EventEmitter } from '@angular/core';

export interface IToolbarOptions {
  show: {prev: boolean, next: boolean}
}

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.less']
})
export class ToolbarComponent {
  @Input() options: IToolbarOptions;
  @Output() onDirEvent = new EventEmitter();
}
