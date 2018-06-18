function initBlockOnSubmit() {
    $('form').on('submit', function () {
        var control = $(this).find('.js-disable-submit');
        if (!control.length) {
            return;
        }
        control[0].disabled = true;
    });
}

export default initBlockOnSubmit;