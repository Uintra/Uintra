import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-first-order-item',
  templateUrl: './first-order-item.component.html',
  styleUrls: ['./first-order-item.component.less']
})
export class FirstOrderItemComponent implements OnInit {
  @Input() item;

  constructor() { }

  ngOnInit() {
  }

}
