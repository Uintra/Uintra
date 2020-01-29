/// <reference types="@types/googlemaps" />
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GoogleGeolocationService {

  getAddress(lat, lng, callback) {
    const geoCoder = new google.maps.Geocoder;

    geoCoder.geocode(
      {
        location: {
          lat,
          lng
        }
      }, (results = [], status) => {
        if (status === 'OK' && results.length) {
          callback(results[0].formatted_address);
        }
      });
  }
}
