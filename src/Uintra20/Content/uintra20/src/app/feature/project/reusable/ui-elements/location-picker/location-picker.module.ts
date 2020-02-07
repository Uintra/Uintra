import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocationPickerComponent } from './location-picker.component';
import { AgmCoreModule } from '@agm/core';
import { GOOGLE_MAPS_CONFIG } from 'src/app/constants/maps/google-maps.const';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    LocationPickerComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    AgmCoreModule.forRoot({
      apiKey: GOOGLE_MAPS_CONFIG.API_KEY,
      libraries: ['geometry', 'places']
    }),
  ],
  exports: [
    LocationPickerComponent
  ]
})
export class LocationPickerModule { }
