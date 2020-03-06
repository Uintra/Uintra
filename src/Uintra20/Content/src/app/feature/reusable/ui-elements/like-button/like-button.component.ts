import { Component, OnInit, Input } from "@angular/core";
import { LikeButtonService, IAddLikeRequest } from "./like-button.service";
import { ILikeData, IUserLikeData } from "./like-button.interface";

@Component({
  selector: "app-like-button",
  templateUrl: "./like-button.component.html",
  styleUrls: ["./like-button.component.less"]
})
export class LikeButtonComponent implements OnInit {
  @Input() likeData: ILikeData;
  @Input() isDisabled: boolean = false;
  newLikesCount: number = null;
  listOfUsersWhoLiked: Array<IUserLikeData> = [];

  get likesCount() {
    return this.newLikesCount || "Like";
  }

  constructor(private likeButtonService: LikeButtonService) {}

  ngOnInit() {
    if (this.likeData && this.likeData.likes) {
      this.newLikesCount = this.likeData.likes.length;
      this.listOfUsersWhoLiked = this.likeData.likes;
    }
  }

  onClickLike() {
    const canAddLike = this.likeData.likedByCurrentUser === false;
    const data: IAddLikeRequest = {
      entityId: this.likeData.id,
      entityType: this.likeData.activityType
    };

    return canAddLike ? this.addLike(data) : this.removeLike(data);
  }

  addLike(data) {
    this.likeButtonService
      .addLike(data)
      .then((response: Array<IUserLikeData>) => {
        this.listOfUsersWhoLiked = response;
      });
    this.newLikesCount += 1;
    this.likeData.likedByCurrentUser = true;
  }

  removeLike(data) {
    this.likeButtonService
      .removeLike(data)
      .then((response: Array<IUserLikeData>) => {
        this.listOfUsersWhoLiked = response;
      });
    this.newLikesCount = this.newLikesCount > 1 ? this.newLikesCount - 1 : 0;
    this.likeData.likedByCurrentUser = false;
  }
}
