  import { Injectable, Inject, NgModuleFactoryLoader, Injector, ComponentFactory } from '@angular/core';
  import { DYNAMIC_COMPONENT_MANIFESTS } from './dynamic-component-loader.module';
  import { DynamicComponentManifest, DYNAMIC_COMPONENT } from './dynamic-component.manifest';
  import {from, Observable, of} from 'rxjs';


  @Injectable({
    providedIn: 'root'
  })
  export class DynamicComponentLoaderService {

    constructor(
      @Inject(DYNAMIC_COMPONENT_MANIFESTS) private manifest: DynamicComponentManifest[],
      private loader: NgModuleFactoryLoader,
      private injector: Injector
    ) { }

  getComponentFactory<T>(componentId: string, injector?: Injector): Observable<ComponentFactory<T>>
  {
      const manifest = this.manifest.find(m => m.componentId === componentId);
      
      if (!manifest) return of(null);

      const p = this.loader.load(manifest.loadChildren)
        .then(ngModuleFactory => {
          const moduleRef = ngModuleFactory.create(injector || this.injector);

          // Read from the moduleRef injector and locate the dynamic component type
          const dynamicComponentType = moduleRef.injector.get(DYNAMIC_COMPONENT);
          // Resolve this component factory
          return moduleRef.componentFactoryResolver.resolveComponentFactory<T>(dynamicComponentType);
        });

      return from(p);
    }

    getDataMapperFor(componentId: string)
    {
      const manifest = this.manifest.find(m => m.componentId === componentId);
      return manifest && manifest.dataMapper || null;
    }
  }
