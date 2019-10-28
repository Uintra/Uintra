import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'app-dots',
  templateUrl: './dots.component.html',
  styleUrls: ['./dots.component.less']
})
export class DotsComponent implements OnInit {

  @Input() count: number = 0;
  @Output() onNextIndex = new EventEmitter();
  @Input() current: number;

  dots: {index: number}[];

  constructor() { }

  ngOnInit() 
  {
    this.dots = this.makeDots(this.count);  
  }

  private makeDots(count: number): { index: number; }[] 
  {
    return (new Array(count)).fill(undefined).map((i, index) => ({index}));
  }

}
