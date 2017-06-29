var Sortable = require('sortablejs');
import helpers from "./../../Core/Content/scripts/Helpers";

var controller = {
    init: function () {
        var container = document.getElementById('js-myLinks-sortable');
        if (!container) {
            return;
        }

        var addControlBtn = document.querySelector('.js-myLinks-add-btn');
        var removeLinks = document.querySelectorAll('.js-myLinks-remove');
        var currentPageID = addControlBtn.getAttribute('data-content-id');
        var className = '_disabled';
        var myLinksState = helpers.localStorage.getItem("myLinks") || {};
        var opener = $('.js-mylinks__opener');
        var activeClass = '_expand';

        opener.on('click', function(e){
            toggleLinks(this);
        });

        getNavState();

        var sortable = Sortable.create(container, {
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

        attachEvents();

        addControlBtn.addEventListener('click', function(e){
            toggleLinks(this, e, 'Add');
        });

        function toggleLinks(element, event, action){
            event.preventDefault();
            $.ajax({
                type: "POST",
                data: {contentId: element.getAttribute('data-content-id')},
                url: '/umbraco/surface/MyLinks/' + action,
                success: function (data) {
                    container.innerHTML = data;
                    removeLinks = document.querySelectorAll('.js-myLinks-remove');
                    attachEvents();
                    if(element.getAttribute('data-content-id') == currentPageID){
                        addControlBtn.classList.toggle(className);
                    }
                }
            });
        }

        function attachEvents(){
            for(var i = 0; i < removeLinks.length; i++){
                removeLinks[i].addEventListener('click', function(e){
                    toggleLinks(this, e, 'Remove');
                });
            }
        }

        function getNavState(){
            var navItem = $('.js-mylinks__item');
            var id = $(navItem).data("id");
    
            if(!jQuery.isEmptyObject(myLinksState)){
                for(var item in myLinksState){
                    $(navItem).toggleClass(activeClass, myLinksState[item]);
                }
            }
        }

        function toggleLinks(el){
            var item = $(el).closest('.js-mylinks__item');
            var itemId = item.data("id");
            var isExpanded;

            item.toggleClass(activeClass);
            isExpanded = item.hasClass(activeClass);

            myLinksState[itemId] = isExpanded;

            helpers.localStorage.setItem("myLinks", myLinksState);
        }
    }
}

export default controller;