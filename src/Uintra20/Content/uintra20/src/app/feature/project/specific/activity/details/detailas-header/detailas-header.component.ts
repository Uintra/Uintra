import { Component, OnInit, Input } from '@angular/core';
import { ISocialDetails } from '../../activity.interfaces';

@Component({
  selector: 'app-detailas-header',
  templateUrl: './detailas-header.component.html',
  styleUrls: ['./detailas-header.component.less']
})
export class DetailasHeaderComponent implements OnInit {
  @Input() details: ISocialDetails;
  @Input() activityName: string;

  constructor() { }

  ngOnInit() {
  }

}
