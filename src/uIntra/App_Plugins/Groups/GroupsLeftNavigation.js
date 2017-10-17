import helpers from "./../Core/Content/scripts/Helpers";

var controller = {
    init: function () {
        var container = document.getElementById('groupsContainer');
        if (!container) {
            return;
        }

        var groupsState = helpers.localStorage.getItem("groups") || {};
        var opener = $('.js-groups__opener');
        var activeClass = '_expand';

        opener.on('click', function (e) {
            e.preventDefault();
            toggleLinks(this);
        });

        getNavState();

        function getNavState() {
            var navItem = $('.js-groups__item');
            var id = $(navItem).data("id");

            if (!jQuery.isEmptyObject(groupsState)) {
                for (var item in groupsState) {
                    $(navItem).toggleClass(activeClass, groupsState[item]);
                }
            }
        }

        function toggleLinks(el) {
            var item = $(el).closest('.js-groups__item');
            var itemId = item.data("id");
            var isExpanded;

            item.toggleClass(activeClass);
            isExpanded = item.hasClass(activeClass);

            groupsState[itemId] = isExpanded;

            helpers.localStorage.setItem("groups", groupsState);
        }
    }
}

export default controller;