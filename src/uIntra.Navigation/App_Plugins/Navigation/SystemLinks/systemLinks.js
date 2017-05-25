import appInitializer from "./../../Core/Content/scripts/AppInitializer";

var active = "_expand";

appInitializer.add(function() {
    $(".js-systemLinks__opener").on("click", function() {
        $(this).toggleClass(active);
    });
});