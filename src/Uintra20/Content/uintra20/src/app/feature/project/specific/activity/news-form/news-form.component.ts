import { Component, OnInit, Input } from "@angular/core";
import ParseHelper from "src/app/feature/shared/helpers/parse.helper";
import { IActivityCreatePanel } from "src/app/ui/panels/activity-create/activity-create-panel.interface";
import { IPinedData } from "../pin-activity/pin-activity.component";
import { ISelectItem } from "../../../reusable/inputs/select/select.component";
import { CreateActivityService } from "src/app/services/createActivity/create-activity.service";

@Component({
  selector: "app-news-form",
  templateUrl: "./news-form.component.html",
  styleUrls: ["./news-form.component.less"]
})
export class NewsFormComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  panelData: any; //TODO create interface
  files: Array<any> = [];
  isPinCheked: boolean;
  tags: any[];
  owners: ISelectItem[];
  defaultOwner: ISelectItem;
  pinDate: IPinedData;

  constructor(private newsCreateService: CreateActivityService) {}

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.tags = Object.values(this.panelData.tags.userTagCollection);
    this.owners = this.getOwners();
    this.defaultOwner = this.owners.find(
      x => x.id == this.panelData.creator.id
    );
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
    // const requestModel: INewsCreateModel = {
    // };
    // this.newsCreateService
    //   .submitNewsContent({
    //     //TODO Add model
    //   });
  }

  editPinDate(pinDate: IPinedData) {
    this.pinDate = pinDate && pinDate.isPinCheked ? pinDate : null;
  }

  private getOwners(): ISelectItem[] {
    return [
      ...this.getMembers(),
      {
        id: this.panelData.creator.id,
        text: this.panelData.creator.displayedName
      }
    ];
  }

  private getMembers(): ISelectItem[] {
    const members = Object.values(this.panelData.members) as Array<any> || [];

    return members.map(member => ({
      id: member.id,
      text: member.displayedName
    }));
  }
}
