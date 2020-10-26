import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import {
  IGroupDocumentResponse,
  IGroupDocument
} from "./document-table.interface";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class DocumentTableService {
  groupDocumentApi = "ubaseline/api/GroupDocuments";

  constructor(private http: HttpClient) {}

  removeDocument(fileId: string, groupId: string) {
    return this.http.delete(
      this.groupDocumentApi + `/Delete?groupId=${groupId}&documentId=${fileId}`
    );
  }

  getGroupDocuments(groupId: string): Observable<IGroupDocument[]> {
    return this.http
      .get<IGroupDocumentResponse[]>(
        this.groupDocumentApi + `/List?groupId=${groupId}`
      )
      .pipe(map(documents => this.flatResponse(documents)));
  }

  flatResponse(documents: IGroupDocumentResponse[]): IGroupDocument[] {
    return documents.map(document => ({
      ...document,
      displayedName: document.creator.displayedName,
      photo: document.creator.photo
    }));
  }
}
