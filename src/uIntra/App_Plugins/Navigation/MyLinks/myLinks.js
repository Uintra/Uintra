var Sortable = require('sortablejs');
import helpers from "./../../Core/Content/scripts/Helpers";
import ajax from "./../../Core/Content/scripts/Ajax";

require("./myLinks.css");

var controller = {
    init: function () {
        var container = document.getElementById('js-myLinks-sortable');
        if (!container) {
            return;
        }

        var addControlBtn = document.querySelector('.js-myLinks-add-btn');
        var currentLinkId = addControlBtn.dataset.mylinkId;
        var className = '_disabled';
        var myLinksState = helpers.localStorage.getItem("myLinks") || {};
        var $opener = $('.js-mylinks__opener');
        var activeClass = '_expand';
        var $myLinksItem = $(".js-mylinks__item");

        $opener.on('click', function (e) {
            toggleLinks(this);
        });

        getNavState();

        Sortable.create(container, {
            group: "myLinksSortable",
            store: {
                /**
                 * Get the order of elements. Called once during initialization.
                 * @param   {Sortable}  sortable
                 * @returns {Array}
                 */
                get: function (sortable) {
                    var order = localStorage.getItem(sortable.options.group.name);
                    return order ? order.split('|') : [];
                },

                /**
                 * Save the order of elements. Called onEnd (when the item is dropped).
                 * @param {Sortable}  sortable
                 */
                set: function (sortable) {
                    var order = sortable.toArray();
                    localStorage.setItem(sortable.options.group.name, order.join('|'));
                }
            }
        });

        $opener.toggleClass("_hide", container.childElementCount == 0);
        initRemoveLinks(container);
        addControlBtn.addEventListener('click', function (e) {
            e.preventDefault();
            let data = { contentId: this.dataset.contentId };
            let url = this.dataset.url;
            ajax.post(url, data).then(function (data) {                
                currentLinkId = data.data.Id;
                reloadList(container);
                if (!$myLinksItem.hasClass(activeClass)) {
                    $myLinksItem.addClass(activeClass);
                    myLinksState[currentLinkId] = true;
                    helpers.localStorage.setItem("myLinks", myLinksState);
                }
                addControlBtn.classList.toggle(className);
            });
        });

        function getNavState() {
            var navItem = $('.js-mylinks__item');
            $(navItem).data("id");

            if (!jQuery.isEmptyObject(myLinksState)) {
                for (var item in myLinksState) {
                    $(navItem).toggleClass(activeClass, myLinksState[item]);
                }
            }
        }

        function toggleLinks(el) {
            var item = $(el).closest('.js-mylinks__item');
            var itemId = item.data("id");

            item.toggleClass(activeClass);
            var isExpanded = item.hasClass(activeClass);

            myLinksState[itemId] = isExpanded;

            helpers.localStorage.setItem("myLinks", myLinksState);
        }

        function initRemoveLinks(container) {
            var removeLinks = container.querySelectorAll('.js-myLinks-remove');
            for (var i = 0; i < removeLinks.length; i++) {
                removeLinks[i].addEventListener('click', function (e) {
                    e.preventDefault();
                    var link = this;
                    var url = this.dataset.url;
                    ajax.delete(url).then(function () {                        
                        reloadList(container);
                        if (link.dataset.id == currentLinkId) {
                            addControlBtn.classList.toggle(className);
                        }
                    });
                });
            }
        }

        function reloadList(container) {
            $.get({ url: container.dataset.url, cache: false }, function (response) {
                let data = response;
                container.innerHTML = data;
                $opener.toggleClass("_hide", container.childElementCount == 0);
                initRemoveLinks(container);
            });
        }
    }
}

export default controller;