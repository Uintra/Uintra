import helpers from "./../../Core/Content/scripts/Helpers";

var controller = {
    init: function () {
        var container = document.getElementById('systemLinksContainer');
        if (!container) {
            return;
        }

        var myLinksState = helpers.localStorage.getItem("systemLinks") || {};
        var opener = $('.js-systemLinks__opener');
        var activeClass = '_expand';

        opener.on('click', function(e){
            toggleLinks(this);
        });

        getNavState();

        function getNavState(){
            var navItem = $('.js-systemLinks__item');
            var id = $(navItem).data("id");
    
            if(!jQuery.isEmptyObject(myLinksState)){
                for(var item in myLinksState){
                    $(navItem).toggleClass(activeClass, myLinksState[item]);
                }
            }
        }

        function toggleLinks(el){
            var item = $(el).closest('.js-systemLinks__item');
            var itemId = item.data("id");
            var isExpanded;

            item.toggleClass(activeClass);
            isExpanded = item.hasClass(activeClass);

            myLinksState[itemId] = isExpanded;

            helpers.localStorage.setItem("systemLinks", myLinksState);
        }
    }
}

export default controller;