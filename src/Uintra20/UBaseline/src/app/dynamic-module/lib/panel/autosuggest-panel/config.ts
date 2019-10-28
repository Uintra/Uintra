import { InjectionToken } from '@angular/core';

export const AUTOSUGGEST_CONFIG = new InjectionToken<IAutosuggestModuleConfig>("CONFIG");
export interface IAutosuggestModuleConfig {
  endPoint: string;
  page: number;
  searchPageUrl: string;
}