import { Component, OnInit, Input } from '@angular/core';
import { IActivityCreatePanel } from '../../activity-create-panel.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { ISelectItem } from 'src/app/feature/project/reusable/inputs/select/select.component';

export interface INewsOwner{
  id: string;
  displayedName: string;
}

@Component({
  selector: 'app-news-create',
  templateUrl: './news-create.component.html',
  styleUrls: ['./news-create.component.less']
})
export class NewsCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  panelData: any; //TODO create interface
  files: Array<any> = [];
  isPinCheked: boolean;
  tags: any[];
  owners: ISelectItem[];
  defaultOwner: ISelectItem;

  constructor() { }

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.tags = Object.values(this.panelData.tags.userTagCollection);
    this.owners = this.getOwners();
    this.defaultOwner = this.owners.find(x => x.id == this.panelData.creator.id);
    debugger
  }

  onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
  }

  onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => {
      const fileElement = file[0];
      return fileElement !== removedFile;
    });
  }

  setValue(value) {
    // debugger;
  }

  onSubmit() {
    // debugger;
  }

  private getOwners(): ISelectItem[] {
    const result = new Array<ISelectItem>();
    if (this.panelData.members) {
      const members = Object.values(this.panelData.members) as Array<any>;
      members.forEach(element => {
        result.push({id: element.id, text: element.displayedName});
      });
      return result;
    }
    result.push({id: this.panelData.creator.id, text: this.panelData.creator.displayedName});
    return result;
  }
}
