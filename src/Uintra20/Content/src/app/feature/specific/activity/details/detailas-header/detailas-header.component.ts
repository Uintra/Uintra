import { Component, OnInit, Input } from '@angular/core';
import { ISocialDetails, ILocation } from '../../activity.interfaces';

@Component({
  selector: 'app-detailas-header',
  templateUrl: './detailas-header.component.html',
  styleUrls: ['./detailas-header.component.less']
})
export class DetailasHeaderComponent implements OnInit {
  @Input() details: ISocialDetails;
  @Input() activityName: string;
  @Input() location?: ILocation;

  googleMapUrl: string;

  constructor() { }

  ngOnInit() {
    if (this.location && this.location.address) {
      this.googleMapUrl = `http://maps.google.co.uk/maps?q=${this.location.address}`;
    }
  }

  getDates() {
    if (!this.details.headerInfo.dates) {
      return "";
    }
    return Object.values(this.details.headerInfo.dates).join(" - ");
  }

}
