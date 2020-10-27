export interface ITablePanel {
  contentTypeAlias: string;
  title?: string;
  table: {
    cells: Array<ICellData[]>;
    useFirstRowAsHeader: boolean;
  }
  anchor?: string;
  panelSettings?: any;
  utmConfiguration?: any;
}
export interface ICellData {
  align: string;
  plainText: string;
  value: string;
}