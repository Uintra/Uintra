import actionLinkWithConfirm from "./../../../Core/Content/scripts/ActionLinkWithConfirm";
import fileUploadController from "./../../../Core/Controls/FileUpload/file-upload";

var holder;
var mobileMediaQuery = window.matchMedia("(max-width: 899px)");

function initMobileTable(){
    var tableHolder = document.querySelector(".table-holder");
    var table = tableHolder.querySelector(".documents-table");

    if(tableHolder.offsetWidth - table.offsetWidth < 0){
        tableHolder.classList.add("_isMobile");

        tableHolder.addEventListener('touchstart', function(e){
            tableHolder.classList.add("_isTouched");
        }, false);
    }
}

function groupDocumentsAfterReload() {
    actionLinkWithConfirm();
}

var controller = {
    init: function () {
        holder = $("#js-group-documents");
        if (!holder.length) {
            return;
        }
        fileUploadController.init(holder);
        
        if (mobileMediaQuery.matches) {
            initMobileTable();
        }
    }
}

uIntra.methods.add("groupDocumentsAfterReload", groupDocumentsAfterReload);

export default controller;
