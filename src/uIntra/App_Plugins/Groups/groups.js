import createGroup from './Create/create-group';
import editGroup from './Edit/edit-group';
import listGroup from './List/group-list';
import groupMembers from './Room/Members/group-members';
import groupSubscribe from './GroupSubscribe';
import groupsLeftNavigation from './GroupsLeftNavigation';
import groupDocuments from './Room/Documents/group-documents';
import pinActivity from './../Core/Content/scripts/PinActivity';


require("./groups.css");

var initSubmitButton = function () {
    var holder = $('#js-group-create-page');

    var form = holder.find('#form');
    var btn = holder.find('._submit');

    btn.click(function (event) {
        if (!form.valid()) {
            event.preventDefault();
            const labelHeight = 26;
            const header = $('#header');
            const additionalHeight = header.length > 0 ? header.outerHeight() + labelHeight : labelHeight;
            const invalidELPos = $('.input-validation-error').first().offset().top;
            window.scrollTo(0, invalidELPos - additionalHeight);
            return;
        }

        if (!pinActivity.isPinAccepted(holder)) {
            event.preventDefault();
            return;
        }

        $(this).addClass('_loadin');
        form.submit();
    });
};

export default function () {
    initSubmitButton();
    createGroup.init();
    editGroup.init();
    listGroup.init();
    groupMembers.init();
    groupSubscribe.init();
    groupsLeftNavigation.init();
    groupDocuments.init();
}