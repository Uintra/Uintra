import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-second-order-item',
  templateUrl: './second-order-item.component.html',
  styleUrls: ['./second-order-item.component.less']
})
export class SecondOrderItemComponent implements OnInit {
  @Input() item;

  constructor() { }

  ngOnInit() {
  }

}
