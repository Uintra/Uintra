import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";

var holder;
 
var controller = {
    init: function () {
        holder = $('#js-group-create-page');

        if (!holder.length) {
            return;
        }
            
        fileUploadController.init(holder);
    }
}

export default controller;
