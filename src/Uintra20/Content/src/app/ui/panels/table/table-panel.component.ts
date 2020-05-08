import { Component, HostBinding, ViewEncapsulation } from '@angular/core';
import { ITablePanel, ICellData } from '../../../shared/interfaces/panels/table/table-panel.interface';

@Component({
  selector: 'table-panel',
  templateUrl: './table-panel.html',
  styleUrls: ['./table-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class TablePanel {
  public data: ITablePanel;

  headerRow: Array<ICellData>;

  @HostBinding('class') rootClasses;


  constructor() { }

  ngOnInit(): void {
    this.checkFirstRowAsHeader();

    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    `;
  }

  private checkFirstRowAsHeader(): void {
    if(this.data.table.useFirstRowAsHeader) {
      this.headerRow = this.data.table.cells[0];
      this.data.table.cells.shift();
    }
  }

  public getAlignClass(cell: ICellData): string {
    switch (cell.align) {
      case 'cell-left': return 'left';
      case 'cell-right': return 'right';
      case 'cell-center': return 'center';
      default: return '';
    }
  }
}
