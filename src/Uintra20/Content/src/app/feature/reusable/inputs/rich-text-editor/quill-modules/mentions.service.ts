import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import Quill from "quill";

@Injectable({
  providedIn: "root"
})
export class MentionsService {
  constructor(private http: HttpClient) {}

  getMentionsModule() {
    return {
      allowedChars: /^[A-Za-z\sÅÄÖåäö]*$/,
      mentionDenotationChars: ["@", "#"],
      source: (searchTerm, renderList) => {
        let matches = [];

        if (searchTerm.length === 0) {
          return;
        } else {
          this.http
            .get("/ubaseline/api/Mention/SearchMention?query=" + searchTerm)
            .subscribe((response: any) => {
              if (response) {
                for (const user of response) {
                  const data = user;
                  data.id = null;
                  data.link = data.url.originalUrl;
                  matches.push(data);
                }
              }
              renderList(matches, searchTerm);
            });
        }
      }
    };
  }
}
