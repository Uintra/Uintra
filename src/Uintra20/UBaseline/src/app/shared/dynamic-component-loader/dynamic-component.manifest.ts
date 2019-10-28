import { InjectionToken } from '@angular/core';

export interface DynamicComponentManifest {
    componentId: string;
    path: string;
    loadChildren: string;
    dataMapper?: {map: (data: any) => any};
}
export const DYNAMIC_COMPONENT = new InjectionToken<any>('DYNAMIC_COMPONENT');