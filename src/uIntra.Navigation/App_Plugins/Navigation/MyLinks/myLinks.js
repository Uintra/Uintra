var Sortable = require('sortablejs');

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

        var sortable = Sortable.create(container, {
            group: "localStorage-example",
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
    }
}

export default controller;