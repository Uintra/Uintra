import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import ajax from "./../../Core/Content/scripts/Ajax";

require("./profile.css");

var initDeleteButton = function (holder) {    
    var btn = holder.find('.js-delete-btn');  

    btn.click(function (event) { 
        var confirmMessage = btn.data('confirm-message');
        if (!confirm(confirmMessage)) {
            return;
        }

        ajax.Delete("/umbraco/surface/Profile/DeletePhoto").then(function(response) {
            location.reload();
        });;
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