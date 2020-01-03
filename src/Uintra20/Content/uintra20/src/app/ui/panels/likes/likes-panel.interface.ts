export interface ILikesPanel {
  activityType: number;
  contentTypeAlias: string;
  canAddLike: boolean;
  count: number;
  entityId: string;
  memberId: string;
  users: Array<{}>;
}
