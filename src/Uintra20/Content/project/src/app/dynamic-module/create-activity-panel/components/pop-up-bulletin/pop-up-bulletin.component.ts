import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-pop-up-bulletin',
  templateUrl: './pop-up-bulletin.component.html',
  styleUrls: ['./pop-up-bulletin.component.less']
})
export class PopUpBulletinComponent implements OnInit {
  @Input() tags: any;

  constructor() { }

  ngOnInit() {
  }

}
