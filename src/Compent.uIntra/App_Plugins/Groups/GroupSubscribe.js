require("./_groups.css");

var controller = {
    initOnLoad: function () {
        this.init('.js-group-subscribe');
    },
    init: function (selector) {
        var holders = $(selector);

        if (!holders.length) {
            return;
        }

        holders.each(function () {
            init($(this));
        });
    },

    changeCount: function (count) {
        var countHolder = $('#membersCount');
        $(countHolder).text(count);
    }
}

export default controller;
