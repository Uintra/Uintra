import { NgModule, NgModuleFactoryLoader, SystemJsNgModuleLoader, InjectionToken } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DynamicComponentManifest } from './dynamic-component.manifest';
import { ROUTES } from '@angular/router';

export const DYNAMIC_COMPONENT_MANIFESTS = new InjectionToken<any>('DYNAMIC_COMPONENT_MANIFESTS');

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [{provide: NgModuleFactoryLoader, useClass: SystemJsNgModuleLoader}]
})
export class DynamicComponentLoaderModule {
  static forRoot(manifest: DynamicComponentManifest[])
  {
    return {
      ngModule: DynamicComponentLoaderModule,
      providers: [
        {provide: ROUTES, useValue: manifest, multi: true},
        {provide: DYNAMIC_COMPONENT_MANIFESTS, useValue: manifest}
      ]
    }
  }
}
