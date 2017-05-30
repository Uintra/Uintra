﻿import appInitializer from "./AppInitializer";

function initBlockOnSubmit() {
    $('form').on('submit', function () {
        var control = $(this).find('.js-disable-submit');
        if (!control.length) {
            return;
        }
        control[0].disabled = true;
    });
}

appInitializer.add(function () {
    initBlockOnSubmit();
});

export default initBlockOnSubmit;