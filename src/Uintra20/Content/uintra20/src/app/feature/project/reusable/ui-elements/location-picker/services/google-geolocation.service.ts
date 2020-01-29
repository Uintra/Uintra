/// <reference types="@types/googlemaps" />
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GoogleGeolocationService {
  private geoCoder;

  constructor() {
    this.geoCoder = new google.maps.Geocoder;
  }

  public getAddress(latitude, longitude): any {
    this.geoCoder.geocode(
      {
        'location': {
          lat: latitude,
          lng: longitude
        }
      }, (results, status) => {
        return results[0].formatted_address;
      });
  }
}
