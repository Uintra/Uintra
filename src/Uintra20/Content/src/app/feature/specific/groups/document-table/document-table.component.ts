import { Component, OnInit, Input } from "@angular/core";
import { DocumentTableService } from "./document-table.service";
import { IGroupDocument } from "./document-table.interface";
import { UintraGroupsService } from "src/app/ui/pages/uintra-groups/documents/uintra-groups-documents-page.service";

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

  constructor(
    private documentTableService: DocumentTableService,
    private uintraGroupsService: UintraGroupsService
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
      console.log(this.documents);
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
    if (confirm("Are you sure?")) {
      this.documentTableService
        .removeDocument(id, this.groupId)
        .subscribe(r => {
          this.getDocuments();
        });
    }
  }
}
