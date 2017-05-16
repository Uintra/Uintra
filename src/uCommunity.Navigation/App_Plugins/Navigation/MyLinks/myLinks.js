import appInitializer from "./../../Core/Content/scripts/AppInitializer";

var active = "_expand";

var controller = {
    switchLinkIcon: function() {
        $('#mylinkIcon').find('span').toggleClass('_isLinked');
        addListeners();
    }
}

function addListeners() {
    $(".js-my-links__opener").on("click", function() {
        $(this).toggleClass(active);
    });
}

appInitializer.add(addListeners);

export default controller;