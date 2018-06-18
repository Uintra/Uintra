import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import ajax from "./../../Core/Content/scripts/Ajax";
import confirm from "./../../Core/Controls/Confirm/Confirm";
var alertify = require('alertifyjs/build/alertify.min');

require("./profile.css");

var initDeleteButton = function (holder) {
    var btn = holder.find('.js-delete-btn');

    btn.click(function () {
        var confirmMessage = btn.data('confirm-message');
        var photo = btn.data('photo');
        confirm.showConfirm('', confirmMessage,
            function () {
                ajax.Delete("/umbraco/surface/Profile/DeletePhoto?photoPath=" + photo).then(function (response) {
                    location.reload();
                });
            }, function () { }, confirm.defaultSettings);
    });
}

function initListeners() {
    $('#js-member-notifier-setting').on('change',
        function (event) {

            let element = event.currentTarget;
            let confirmMessage = $(element).attr('dataConfirmMessage');
            confirm.showConfirm('',
                confirmMessage,
                function () {
                    let notifierType = element.attributes.notifiertype.value;
                    let value = element.checked;
                    SetNotifierSetting(notifierType, value);
                },
                function () {
                    element.checked = !element.checked;
                },
                confirm.defaultSettings);
        });
}

function SetNotifierSetting(notifierType, value) {
    $.ajax({
        type: "POST",
        url: "/umbraco/api/MemberNotifierSettings/Update?type=" + notifierType + "&isEnabled=" + value,
        complete: () => alertify.success('Settings has been saved.')
    });

}

let controller = {
    init: function () {
        let holder = $('#js-profile-page');
        if (!holder.length) {
            return;
        }
        initListeners();
        initDeleteButton(holder);
        fileUploadController.init(holder);
    }
}

export default controller;