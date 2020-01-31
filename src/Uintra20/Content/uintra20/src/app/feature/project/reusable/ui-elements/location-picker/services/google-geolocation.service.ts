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
          callback({
            address: results[0].formatted_address,
            shortAddress: results[0].address_components[2].long_name
          });
        }
      });
  }
}
