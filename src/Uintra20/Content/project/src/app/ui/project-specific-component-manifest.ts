import { DynamicComponentManifest } from '../shared/dynamic-component-loader/dynamic-component.manifest';

export const specificComponents: DynamicComponentManifest[] = [
    // example
    // {
    //     componentId: 'cookie',
    //     path: 'dynamic-module',
    //     loadChildren: './dynamic-module/cookie/cookie.module#CookieModule'
    // },
    {
        componentId: 'activityCreatePanel',
        path: 'dynamic-module',
        loadChildren: './dynamic-module/create-activity-panel/create-activity-panel.module#CreateActivityPanelModule'
    }
]
