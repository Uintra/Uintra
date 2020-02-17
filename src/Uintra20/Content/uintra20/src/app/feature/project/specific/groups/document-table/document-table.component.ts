import { Component, OnInit } from "@angular/core";
import { DocumentTableService } from "./document-table.service";
import { IGroupDocument } from "./document-table.interface";

@Component({
  selector: "app-document-table",
  templateUrl: "./document-table.component.html",
  styleUrls: ["./document-table.component.less"]
})
export class DocumentTableComponent implements OnInit {
  documents: IGroupDocument[] = [];

  isAsc: boolean = true;
  sortedBy: string;

  tableHeader = [
    {
      text: "Document type",
      key: "type"
    },
    {
      text: "Document",
      key: "name"
    },
    {
      text: "Added by",
      key: "displayedName"
    },
    {
      text: "Date",
      key: "createDate"
    }
  ];

  constructor(private documentTableService: DocumentTableService) {}

  ngOnInit() {
    this.documentTableService
      .getGroupDocuments("dca05b7e-26b2-476b-8194-e307406711a3")
      .subscribe(r => {
        this.documents = r;
      });

    this.onSort("name");
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
    this.documentTableService.removeDocuments(id, "dca05b7e-26b2-476b-8194-e307406711a3");
  }
}
