import { IPanelSettings } from '../interface/panel-settings';

export function resolveThemeCssClass(settings: IPanelSettings) {
  return (settings && settings.theme && settings.theme.value && settings.theme.value.alias) || 'default-theme';
}