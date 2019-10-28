import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { resolveThemeCssClass } from '../lib/helper/panel-settings';
import { IPanelSettings } from 'src/app/shared/interface/panel-settings';

interface ITablePanelData {
  title: IUProperty<string>;
  table: IUProperty<ITableData>;
  panelSettings: IPanelSettings;
}
interface ITableData {
  cells: Array<Array<ICellData>>;
  columnStyles: Array<any>;
  rowStyles: Array<any>;
  tableStyle: Array<any>;
  useFirstRowAsHeader: boolean;
}
interface ICellData {
  align: string;
  plainText: string;
  value: string;
}

@Component({
  selector: 'app-table-panel',
  templateUrl: './table-panel.component.html',
  styleUrls: ['./table-panel.component.less']
})
export class TablePanelComponent implements OnInit {
  @Input() data: ITablePanelData;
  @HostBinding('class') get hostClasses() { return resolveThemeCssClass(this.data.panelSettings); }

  headerRow: Array<ICellData>;

  constructor() { }

  ngOnInit() {
    this.checkFirstRowAsHeader();
  }

  checkFirstRowAsHeader() {
    if(this.data.table.value.useFirstRowAsHeader) {
      this.headerRow = this.data.table.value.cells[0];
      this.data.table.value.cells.shift();
    }
  }

  getAlignClass(cell: ICellData): string {
    switch (cell.align) {
      case 'cell-left': return 'left';
      case 'cell-right': return 'right';
      case 'cell-center': return 'center';
      default: return '';
    }
  }
}
