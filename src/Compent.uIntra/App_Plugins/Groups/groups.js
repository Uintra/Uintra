import createGroup from './Create/create-group';
import editGroup from './Edit/edit-group';
import listGroup from './List/group-list';
import groupMembers from './Room/Members/group-members';
import groupSubscribe from './GroupSubscribe';
import groupsLeftNavigation from './GroupsLeftNavigation';
import groupDocuments from './Room/Documents/group-documents';


require("./groups.css");

export default function () {
    createGroup.init();
    editGroup.init();
    listGroup.init();
    groupMembers.init();
    groupSubscribe.init();
    groupsLeftNavigation.init();
    groupDocuments.init();
}