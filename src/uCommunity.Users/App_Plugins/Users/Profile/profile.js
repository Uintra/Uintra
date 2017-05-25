import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";

require("./profile.css");

var holder;

var controller = {
    init: function () {
        holder = $('#js-profile-page');

        if (!holder.length) {
            return;
        }
        fileUploadController.init(holder);
    }
}

window.profileController = controller;