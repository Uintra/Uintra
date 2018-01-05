let initSubscribeNotes = function (holder) {
    let subscribeNotesHolder = holder.find('.js-subscribe-notes-holder');
    let canSubscibeChx = holder.find('.js-can-subscribe');

    if (canSubscibeChx.is(":unchecked")) {
        subscribeNotesHolder.hide();
    }

    canSubscibeChx.on('change', canSubscibeChange);

    function canSubscibeChange() {
        if (canSubscibeChx.is(":checked")) {
            subscribeNotesHolder.show();
        } else {
            subscribeNotesHolder.hide();
        }
    }

    canSubscibeChange();
}

let controller = {
    init: function () {
        let holder = $('.js-subscribe-settings-holder');

        if (!holder.length) return;

        initSubscribeNotes(holder);
    }
}

export default controller;