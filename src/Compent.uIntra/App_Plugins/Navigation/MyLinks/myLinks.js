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
            onUpdate: function (evt) {

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