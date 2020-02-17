export interface IGroupDocumentResponse {
  id: string;
  type: string;
  name: string;
  creator: {
    id: string;
    displayedName: string;
    photo: string;
    photoId: number;
    email: string;
    loginName: string;
  };
  inactive: boolean;
  createDate: string;
  canDelete: boolean;
  fileUrl: string;
}


export interface IGroupDocument {
  id: string;
  type: string;
  name: string;
  displayedName: string;
  photo: string;
  createDate: string;
  canDelete: boolean;
  fileUrl: string;
}
