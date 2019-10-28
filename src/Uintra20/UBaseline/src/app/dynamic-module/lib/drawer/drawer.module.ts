import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerComponent } from './drawer.component';
import { DrawerService } from './service/drawer.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [DrawerComponent],
  imports: [
    CommonModule,
    BrowserAnimationsModule
  ],
  exports: [DrawerComponent]
})
export class DrawerModule {
  static forRoot(): ModuleWithProviders
  {
    return {
      ngModule: DrawerModule,
      providers: [
        DrawerService
      ]
    }
  }
 }
