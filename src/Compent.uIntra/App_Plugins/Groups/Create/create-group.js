import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";

const DATA_STORAGE = $(".js-hidden-group-create-description")[0];
const DESCRIPTION_ELEMENT = $(".js-group-create-description")[0];
const RTE_SETTINGS = {
    modules: {
        toolbar: {
            container: ['emoji', 'bold', 'italic', 'link']
        }
    }
};

var holder;

var controller = {
    init: function () {
        holder = $('#js-group-create-page');

        if (!holder.length) return;

        this.initRte();
        fileUploadController.init(holder);
    },
    initRte: function () {
        helpers.initQuill(DESCRIPTION_ELEMENT, DATA_STORAGE, RTE_SETTINGS);
        $(".ql-emoji").remove(); // Due to bug in Quill
    }
};

export default controller;
