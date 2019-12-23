import { Component, Input, OnInit } from "@angular/core";
import {
  PublicationsService,
  IAddLikeRequest
} from "../helpers/publications.service";

@Component({
  selector: "app-central-feed-publication",
  templateUrl: "./central-feed-publication.component.html",
  styleUrls: ["./central-feed-publication.component.less"]
})
export class CentralFeedPublicationComponent implements OnInit {
  @Input() publication;

  get commentsCount() {
    return this.publication.activity.commentsCount || "Comment";
  }
  get likesCount() {
    return this.newLikesCount || "Like";
  }

  newLikesCount: number = null;
  listOfUsersWhoLiked: Array<string> = [];

  constructor(private publicationsService: PublicationsService) {}

  ngOnInit(): void {
    this.newLikesCount = this.publication.activity.likes.length;
    this.listOfUsersWhoLiked = this.publication.activity.likes;
  }

  getPublicationDate() {
    return this.publication.activity.dates.length
      ? this.publication.activity.dates[0]
      : "";
  }

  onClickLike() {
    const canAddLike = this.publication.activity.likedByCurrentUser === false;

    const data: IAddLikeRequest = {
      entityId: this.publication.activity.id,
      entityType: this.publication.activity.activityType
    };

    return canAddLike ? this.addLike(data) : this.removeLike(data);
  }

  addLike(data) {
    this.publicationsService
      .addLike(data)
      .then((response: Array<string>) => {
      this.listOfUsersWhoLiked = response;
    });
    this.newLikesCount += 1;
    this.publication.activity.likedByCurrentUser = true;
  }

  removeLike(data) {
    this.publicationsService
      .removeLike(data)
      .then((response: Array<string>) => {
        this.listOfUsersWhoLiked = response;
      });
    this.newLikesCount -= 1;
    this.publication.activity.likedByCurrentUser = false;
  }
}
