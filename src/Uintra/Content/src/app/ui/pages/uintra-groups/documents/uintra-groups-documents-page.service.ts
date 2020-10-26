import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class UintraGroupsService {
  groupDocumentApi = "ubaseline/api/GroupDocuments";

  private documentsRefreshTrigger = new Subject();
  documentsRefreshTrigger$ = this.documentsRefreshTrigger.asObservable();

  constructor(private http: HttpClient) {}

  uploadFile(fileId: string, groupId: string) {
    return this.http.post(this.groupDocumentApi + `/Upload`, {
      GroupId: groupId,
      NewMedia: fileId
    });
  }

  refreshDocuments() {
    this.documentsRefreshTrigger.next();
  }
}
