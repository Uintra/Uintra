import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import subscribe from "./../../Subscribe/Subscribe";

var holder;
var state;
var contentContainer;
var linkControls;

var reloadContent = function () {
    var url = "/umbraco/surface/Events/List?type=" + state.type + "&showOnlySubscribed=" + state.showOnlySubscribed;

    $.get(url, function (data) {
        contentContainer.html(data);
    });
}

var setActive = function (control) {
    linkControls.each(function () {
        $(this).removeClass('_active');
    });
    $(control).closest('li').addClass('_active');
}

var initShowOnlyMy = function () {
    var checkbox = $('.js-event-overview-show-subscribed');

    checkbox.on('change', function () {
        var $this = $(this);
        state.showOnlySubscribed = $this[0].checked;

        reloadContent();
    });
}

var initLinks = function () {
    var links = holder.find('.js-event-tabs-link');

    links.on('click', function () {
        var $this = $(this);
        state.type = $this.data('type');

        reloadContent();
        setActive($this);
    });

    linkControls = links.map(function () {
        return $(this).closest('li');
    });
}

var onSubscribe = function () {
    if (state.showOnlySubscribed) {
        reloadContent();
    }
}

var controller = {
    init: function () {
        holder = $('#js-event-overview');
        if (!holder.length) {
            return;
        }

        var stateControl = holder.find('.js-event-overview-state').get(0);

        state = JSON.parse(stateControl.value);
        contentContainer = holder.find('.js-event-overview-list');

        initLinks();
        initShowOnlyMy();
        subscribe.addOnSubscribe(onSubscribe);
    }
}

appInitializer.add(controller.init);