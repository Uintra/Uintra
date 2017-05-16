import appInitializer from "./../../Core/Content/scripts/AppInitializer";

var active = "_expand";

var controller = {
    switchLinkIcon: function() {
        $('#mylinkIcon').find('span').toggleClass('_isLinked');
    }
}

appInitializer.add(function() {
    $(".js-my-links__opener").on("click", function() {
        $(this).toggleClass(active);
    });
});

export default controller;