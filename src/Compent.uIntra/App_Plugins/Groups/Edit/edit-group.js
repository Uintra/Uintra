import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import confirm from "./../../Core/Controls/Confirm/Confirm";
var Alertify = require('alertifyjs/build/alertify.min');

var holder;

var initHideControl = function () {
    var hideControl = holder.find('.js-group-hide');

    var text = hideControl.data('text');
    var textOk = hideControl.data('ok');
    var textCancel = hideControl.data('cancel');
    Alertify.defaults.glossary.cancel = textCancel;
    Alertify.defaults.glossary.ok = textOk;

    hideControl.on('click', function () {
        confirm.showConfirm('',text,
             function () {
                 $.post('/umbraco/surface/Group/Hide?id=' + hideControl.data('id'),
                     function (data) {
                         window.location.href = data;
                     });
             }, function () { }, confirm.defaultSettings);

        return false;
    });
}
var controller = {
    init: function () {
        holder = $('#js-group-edit-page');

        if (!holder.length) {
            return;
        }
            
        initHideControl();
        fileUploadController.init(holder);
    }
}


export default controller;

