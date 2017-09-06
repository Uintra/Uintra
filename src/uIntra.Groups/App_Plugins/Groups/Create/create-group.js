import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";

var holder;
 
var initDescriptionControl = function () {    
    helpers.initActivityDescription(holder, '#js-hidden-description-container', '#description', '.form__btn._submit');    
}


var controller = {
    init: function () {
        holder = $('#js-group-create-page');

        if (!holder.length) {
            return;
        }
            
        initDescriptionControl();
        fileUploadController.init(holder);
    }
}
appInitializer.add(controller.init);
