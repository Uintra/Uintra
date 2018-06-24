require("./Login/login.css");

import profile from './Profile/profile';
import userList from './UserList/userList';
import temp from './UserList/temp';

export default function () {
    profile.init();
    userList.init();
    temp.init();
}