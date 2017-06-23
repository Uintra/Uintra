var Sortable = require('sortablejs');
import ajax from "./../../Core/Content/scripts/Ajax";

var controller = {
    init: function () {
        var container = document.getElementById('js-myLinks-sortable');
        if (!container) {
            return;
        }

        var addControlBtn = document.querySelector('.js-myLinks-add-btn');
        var className = '_disabled';

        Sortable.create(container, {
            onUpdate: function () {

            }
        });

        initRemoveLinks(container);

        addControlBtn.addEventListener('click', function() {
            ajax.Post(this.dataset.url, this.dataset.contentId, function(data) {
                e.preventDefault();
                fillContainerData(container, data);
                addControlBtn.classList.toggle(className);
            });
        });

        function initRemoveLinks(container) {
            var removeLinks = container.querySelectorAll('.js-myLinks-remove');

            for(var i = 0; i < removeLinks.length; i++){
                removeLinks[i].addEventListener('click', function(e) {
                    e.preventDefault();
                    var link = this;
                    var url = this.dataset.url;
                    ajax.Delete(url, function(data) {
                        fillContainerData(container, data);    
                        if (link.dataset.isCurrentPage) {
                            addControlBtn.classList.toggle(className);
                        }
                    });
                });
            }
        }

        function fillContainerData(container, data) {
            container.innerHTML = data;
            initRemoveLinks(container);
        }
    }
}

export default controller;