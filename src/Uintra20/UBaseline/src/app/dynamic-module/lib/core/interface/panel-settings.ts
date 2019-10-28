import { IUProperty } from './umbraco-property';
import { HexColor } from './types';
import { get } from 'lodash';

export interface IPanelSettings {
    behaviour: IUProperty<'full-content'>;
    theme: IUProperty<IPanelTheme>;
}

export interface IPanelTheme {
   alias: string;
   backgroundColor: HexColor;
   buttonColor: HexColor;
   textColor: HexColor;
   titleColor: HexColor;
}

export function resolveThemeCssClass(settings: IPanelSettings)
{
  let theme = get(settings, 'theme.value.alias', 'default-theme');
  let behavior = get(settings, 'behaviour.value', '');

  return `${theme} ${behavior}`;
}
