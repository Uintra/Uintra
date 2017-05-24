import appInitializer from "./../../Core/Content/scripts/AppInitializer";


appInitializer.add(function() {
    var container = $('.js-myLinks-container');
    if (!container) {
        return;
    }

    var addControl = $('.js-myLinks-add');
    var removeControl = $('.js-myLinks-remove');
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

    removeControl.on('click',function() {
        $.ajax({
            type: "POST",
            data: {id: $(this).data("id")},
            url: "/umbraco/surface/MyLinks/Remove",
            success: function (data) {
                container.html(data);
                addControl.show();
                removeControl.hide();
            }
        });
    });
});
