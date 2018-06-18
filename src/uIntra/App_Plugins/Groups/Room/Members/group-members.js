var mobileMediaQuery = window.matchMedia("(max-width: 899px)");

function initMobileTable(){
    var tableHolder = document.querySelector(".table-holder");
    var table = tableHolder.querySelector(".members-table");

    if(tableHolder.offsetWidth - table.offsetWidth < 0){
        tableHolder.classList.add("_isMobile");

        tableHolder.addEventListener('touchstart', function(e){
            tableHolder.classList.add("_isTouched");
        }, false);
    }
}



var controller = {
    init: function () {
        var holder = $("#js-group-members");
        if (!holder.length) {
            return;
        }
        
        if (mobileMediaQuery.matches) {
            initMobileTable();
        }
    }
}
export default controller;