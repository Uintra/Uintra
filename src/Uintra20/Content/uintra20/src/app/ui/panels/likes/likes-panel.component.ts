import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ILikesPanel } from './likes-panel.interface';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'likes-panel',
  templateUrl: './likes-panel.html',
  styleUrls: ['./likes-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LikesPanel implements OnInit {

  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
  ngOnInit(): void {
    console.log(this.data);
  }

}