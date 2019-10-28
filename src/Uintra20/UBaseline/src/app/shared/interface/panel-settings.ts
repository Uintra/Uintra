import { IUProperty } from './umbraco-property';
import { HexColor } from './alias';

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

