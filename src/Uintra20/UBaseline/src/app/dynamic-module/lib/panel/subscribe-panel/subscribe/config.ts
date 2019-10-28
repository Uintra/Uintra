import { InjectionToken } from '@angular/core';

export interface ISubscribeConfig {
   api: string;
   agreementsUrl: string;
}

export const SUBSCRIBE_MODULE_CONFIG = new InjectionToken<ISubscribeConfig>('SUBSCRIBE_MODULE_CONFIG');