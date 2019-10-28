import { IPanelSettings } from "src/app/shared/interface/panel-settings";
import get from "lodash/get";

export function resolveThemeCssClass(settings: IPanelSettings)
{
  let theme = get(settings, 'theme.value.alias', 'default-theme');
  let behavior = get(settings, 'behaviour.value', '');
  
  return `${theme} ${behavior}`; 
}