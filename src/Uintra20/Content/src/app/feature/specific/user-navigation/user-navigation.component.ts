import { Component, OnInit, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs/operators";
import { IUserNavigation } from "../../reusable/ui-elements/comments/components/user-navigation/user-navigation.interface";
import { UserNavigationService } from "./services/user-navigation.service";
import { Subscription } from "rxjs";
import { DownloadedPhotoWatcherService } from "./services/downloaded-photo-watcher.service";

export enum IconType {
  "icon-umbraco-logo" = 1,
  "icon-user-profile",
  "icon-uintra",
  "icon-logout",
}

@Component({
  selector: "user-navigation",
  templateUrl: "./user-navigation.component.html",
  styleUrls: ["./user-navigation.component.less"],
})
export class UserNavigationComponent implements OnInit, OnDestroy {
  private $navigation: Subscription;
  private $redirect: Subscription;

  public inProgress: boolean;
  public data: IUserNavigation;
  public navigationExpanded: boolean;
  public downloadedPhoto: string;
  public isPhotoRemoved: boolean;

  public get isNavigationExpanded() {
    return this.navigationExpanded;
  }

  constructor(
    private userNavigationService: UserNavigationService,
    private downloadedPhotoWatcherService: DownloadedPhotoWatcherService
  ) {}

  public ngOnInit(): void {
    this.$navigation = this.userNavigationService
      .topNavigation()
      .subscribe((next: IUserNavigation) => {
        this.data = next;
      });
    this.downloadedPhotoWatcherService.getTrigger().subscribe((r: string) => {
      if (r) {
        this.downloadedPhoto = r;
        this.isPhotoRemoved = false;
      } else {
        this.downloadedPhoto = '';
        this.isPhotoRemoved = true;
      }
    });
  }

  public ngOnDestroy(): void {
    if (this.$navigation) {
      this.$navigation.unsubscribe();
    }
    if (this.$redirect) {
      this.$redirect.unsubscribe();
    }
  }

  public toggleUserNavigation(event): void {
    event.preventDefault();
    event.stopPropagation();
    this.navigationExpanded = !this.navigationExpanded;
  }

  public closeUserNavigation(): void {
    this.navigationExpanded = false;
  }

  public getClass(type) {
    return IconType[type];
  }

  public redirect(url, type): void {
    this.inProgress = true;

    if (type === 1) {
      this.$redirect = this.userNavigationService
        .redirect(url.originalUrl)
        .pipe(
          finalize(() => {
            this.inProgress = false;
          })
        )
        .subscribe(
          (next) => {
            window.open(window.location.origin + "/umbraco", "_blank");
          },
          (error) => {
            if (error.status === 403) {
              console.error(error.message);
            }
          }
        );
    }

    if (type === 4) {
      this.$redirect = this.userNavigationService
        .redirect(url.originalUrl)
        .pipe(
          finalize(() => {
            this.inProgress = false;
          })
        )
        .subscribe(() => {
          window.location.href = "/login";
        });
    }
  }
}
