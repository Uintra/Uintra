import { Component, ViewEncapsulation, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AppService } from "src/app/app.service";
import { IProfilePage } from "src/app/shared/interfaces/pages/profile/profile-page.interface";
import { Indexer } from "../../../../shared/abstractions/indexer";

@Component({
  selector: "profile-page",
  templateUrl: "./profile-page.html",
  styleUrls: ["./profile-page.less"],
  encapsulation: ViewEncapsulation.None,
})
export class ProfilePage extends Indexer<number> implements OnInit {
  public data: IProfilePage;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private appService: AppService
  ) {
    super();
  }

  public ngOnInit(): void {
    this.activatedRoute.data.subscribe((data: IProfilePage) => {
      if (!data.requiresRedirect) {
        this.data = data;
        this.appService.setPageAccess(data.allowAccess);
      } else {
        this.router.navigate([data.errorLink.originalUrl]);
      }
    });
  }
}
