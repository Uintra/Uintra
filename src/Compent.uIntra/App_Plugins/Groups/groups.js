import createGroup from './Create/create-group';
import groupDocuments from './Documents/group-documents';
import editGroup from './Edit/edit-group';
import listGroup from './List/group-list';
import groupMembers from './Members/group-members';
import groupSubscribe from './GroupSubscribe';

require("./groups.css");

export default function () {
    createGroup.init();
    groupDocuments.init();
    editGroup.init();
    listGroup.init();
    groupMembers.init();
    groupSubscribe.init();
}