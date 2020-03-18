import { Component, OnInit, Input, EventEmitter, Output, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';

@Component({
  selector: 'app-event-subscription',
  templateUrl: './event-subscription.component.html',
  styleUrls: ['./event-subscription.component.less'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => EventSubscriptionComponent), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => EventSubscriptionComponent), multi: true }
  ]
})
export class EventSubscriptionComponent implements OnInit {
  @Input() isChecked: boolean;
  @Input() isSubscribed: boolean;
  @Input() subscribeNotes: string;
  @Input() details: any;
  @Output() checkboxChange = new EventEmitter<boolean>();
  @Output() inputChange = new EventEmitter<string>();
  @Output() subscribeToggle = new EventEmitter();

  constructor() { }

  ngOnInit() {
    this.details = this.details !== undefined;
  }

  onCheckboxChange() {
    this.checkboxChange.emit(this.isChecked);
  }

  onChangeInput(val) {
    this.inputChange.emit(val);
  }

  onSubscribeToggle() {
    this.subscribeToggle.emit();
  }
}
