export interface IProfileEditPage {
    member: IMemberEdit;
    title: string;
}
export interface IMemberEdit {
    id: string;
    firstName: string;
    lastName: string;
    phone: string;
    department: string;
    photo: string;
    photoId: string;
    email: string;
    profileUrl: string;
    mediaRootId: string;
    newMedia: string;
    memberNotifierSettings: string;
}
