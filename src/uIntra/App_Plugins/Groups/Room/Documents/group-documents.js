//var Dropzone = require("dropzone");
//import actionLinkWithConfirm from "../../Core/Content/scripts/ActionLinkWithConfirm";
//var mobileMediaQuery = window.matchMedia("(max-width: 899px)");

//function initMobileTable(){
//    var tableHolder = document.querySelector(".table-holder");
//    var table = tableHolder.querySelector(".documents-table");

//    if(tableHolder.offsetWidth - table.offsetWidth < 0){
//        tableHolder.classList.add("_isMobile");

//        tableHolder.addEventListener('touchstart', function(e){
//            tableHolder.classList.add("_isTouched");
//        }, false);
//    }
//}



//var controller = {
//    init: function () {
//        var holder = $("#js-group-documents");
//        if (!holder.length) {
//            return;
//        }
//        var dropzoneElem = holder.find('.js-dropzone');
//        if (dropzoneElem.length) {

//            var groupId = dropzoneElem.data('groupId');
//            var dropzoneOptions = {
//                url: "/umbraco/Surface/GroupDocuments/Upload?groupId=" + groupId,
//                addRemoveLinks: false,
//                maxFilesize: 50,
//                dictDefaultMessage: dropzoneElem.data('defaultText'),
//                dictRemoveFile: dropzoneElem.data('removeText')
//            }
//            var dropzone = new Dropzone(dropzoneElem[0], dropzoneOptions);

//            dropzone.on('success', function () {
//                holder.find('#js-group-documents-update-lnk').click();
//            });
//        }
//        if (mobileMediaQuery.matches) {
//            initMobileTable();
//        }

//        window.groupDocuments = {
//            afterReload: () => { actionLinkWithConfirm(); }
//        }

//    }
//}


//export default controller;
