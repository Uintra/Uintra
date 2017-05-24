import appInitializer from "./../../Core/Content/scripts/AppInitializer";


appInitializer.add(function() {
    var container = document.querySelector('.js-myLinks-container');
    if (!container) {
        return;
    }

    $('.js-myLinks-add').on('click',function() {
        $.ajax({
            type: "POST",
            data: {contentId: $(this).data("contentId")},
            url: "/umbraco/surface/MyLinks/Add",
            success: function (data) {
                container.html(data);
            }
        });
    });

    $('.js-myLinks-remove').on('click',function() {
        $.ajax({
            type: "POST",
            data: {id: $(this).data("id")},
            url: "/umbraco/surface/MyLinks/Remove",
            success: function (data) {
                container.html(data);
            }
        });
    });
});

export default controller;