import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'ubl-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.less']
})
export class ButtonComponent {

  @Input() disabled: boolean;
  @Input() inProgress: boolean;
  @Input() icon: string;
  @Input() iconPosition: 'before' | 'after';

  @Output() clicked = new EventEmitter();

  get state()
  {
    if (this.disabled) return 'disabled';
    if (this.inProgress) return 'busy';

    return '';
  }

  handleClick(ev: MouseEvent)
  {
    debugger
    console.log('clicked')
  }
}
