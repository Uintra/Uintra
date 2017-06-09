import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import ajax from "./../../Core/Content/scripts/Ajax";
import confirm from "./../../Core/Controls/Confirm/Confirm";

require("./profile.css");

var initDeleteButton = function (holder) {    
    var btn = holder.find('.js-delete-btn');  

    btn.click(function () { 
        var confirmMessage = btn.data('confirm-message');
        confirm.showConfirm(confirmMessage, 
            function () {
                ajax.Delete("/umbraco/surface/Profile/DeletePhoto").then(function(response) {
                    location.reload();
                });
            }, function () { }, confirm.defaultSettings);
    });
}

var controller = {
    init: function () {
        var holder = $('#js-profile-page');
        if (!holder.length) {
            return;
        }

        initDeleteButton(holder);
        fileUploadController.init(holder);
    }
}

appInitializer.add(function() {
    controller.init();
});