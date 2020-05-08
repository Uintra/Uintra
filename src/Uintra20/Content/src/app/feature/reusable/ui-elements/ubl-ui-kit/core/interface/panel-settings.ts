import { HexColor } from './types';
import { IUProperty } from './umbraco-property';

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
