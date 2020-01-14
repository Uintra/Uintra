export interface ILikeData {
  likedByCurrentUser: boolean;
  id: string;
  activityType?: string;
  likes: Array<IUserLikeData>;
}

export interface IUserLikeData {
  user: string;
  userId: string;
}
