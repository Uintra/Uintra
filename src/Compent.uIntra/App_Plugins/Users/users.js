require("./Login/login.css");

import profile from './Profile/profile';
import userList from './UserList/userList';

export default function () {
    profile.init();
    userList.init();
}