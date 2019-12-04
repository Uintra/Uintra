import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-create-activity-bulletins',
  templateUrl: './create-activity-bulletins.component.html',
  styleUrls: ['./create-activity-bulletins.component.less']
})
export class CreateActivityBulletinsComponent implements OnInit {
  @Input() tags: any[];

  constructor() { }
  isPopupShowing: boolean = false;

  onShowPopUp() {
    this.isPopupShowing = true;
  }

  onHidePopUp() {
    this.isPopupShowing = false;
  }

  onSubmitContent() {
    debugger;
  }

  ngOnInit(): void {

  }
}
