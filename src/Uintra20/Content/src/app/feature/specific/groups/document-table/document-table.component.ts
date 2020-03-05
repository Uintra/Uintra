import { Component, OnInit, Input } from "@angular/core";
import { DocumentTableService } from "./document-table.service";
import { IGroupDocument } from "./document-table.interface";
import { UintraGroupsService } from "src/app/ui/pages/uintra-groups/documents/uintra-groups-documents-page.service";
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: "app-document-table",
  templateUrl: "./document-table.component.html",
  styleUrls: ["./document-table.component.less"]
})
export class DocumentTableComponent implements OnInit {
  @Input() groupId: string;
  documents: IGroupDocument[] = [];

  isAsc: boolean = true;
  sortedBy: string;

  tableHeader = [
    {
      text: this.translate.instant('groupDocuments.Table.Type.lbl'),
      key: "type"
    },
    {
      text: this.translate.instant('groupDocuments.Table.Name.lbl'),
      key: "name"
    },
    {
      text: this.translate.instant('groupDocuments.Table.Creator.lbl'),
      key: "displayedName"
    },
    {
      text: this.translate.instant('groupDocuments.Table.Date.lbl'),
      key: "createDate"
    }
  ];

  constructor(
    private documentTableService: DocumentTableService,
    private uintraGroupsService: UintraGroupsService,
    private translate: TranslateService,
  ) {}

  ngOnInit() {
    this.getDocuments();
    this.onSort("name");

    this.uintraGroupsService.documentsRefreshTrigger$.subscribe(() => {
      this.getDocuments();
    });
  }

  getDocuments() {
    this.documentTableService.getGroupDocuments(this.groupId).subscribe(r => {
      this.documents = r;
    });
  }

  onSort(key: string) {
    this.isAsc = key === this.sortedBy ? !this.isAsc : this.isAsc;
    this.sortedBy = key;

    this.documents.sort((a, b) => {
      if (a[key] < b[key]) {
        return this.isAsc ? -1 : 1;
      }

      return this.isAsc ? 1 : -1;
    });
  }

  onRemove(id: string) {
    if (confirm(this.translate.instant('groupDocuments.Delete.ConfirmText.lbl'))) {
      this.documentTableService
        .removeDocument(id, this.groupId)
        .subscribe(r => {
          this.getDocuments();
        });
    }
  }
}
