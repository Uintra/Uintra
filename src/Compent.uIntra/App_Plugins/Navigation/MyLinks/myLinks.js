import appInitializer from "./../../Core/Content/scripts/AppInitializer";

var Sortable = require('sortablejs');

appInitializer.add(function() {
    var container = $('.js-myLinks-container');
    if (!container) {
        return;
    }

    var addControl = $('.js-myLinks-add');
    var removeControl = $('.js-myLinks-remove');

    var el = document.getElementById('js-myLinks-sortable');
    var sortable = Sortable.create(el);

    addControl.on('click',function() {
        $.ajax({
            type: "POST",
            data: {contentId: $(this).data("contentId")},
            url: "/umbraco/surface/MyLinks/Add",
            success: function (data) {
                container.html(data);
                addControl.hide();
                removeControl.show();
            }
        });
    });

    removeControl.on('click',function(event) {
        event.preventDefault();

        $.ajax({
            type: "POST",
            data: {contentId: $(this).data("contentId")},
            url: "/umbraco/surface/MyLinks/Remove",
            success: function (data) {
                container.html(data);
                addControl.show();
                removeControl.hide();
            }
        });
    });
});
