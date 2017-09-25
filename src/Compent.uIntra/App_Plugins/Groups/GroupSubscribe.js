import confirm from './../Core/Controls/Confirm/Confirm';

function unsubscribeEventHanlder(e, title, message) {
    e.preventDefault();

    confirm.showConfirm(title, message, () => e.target.form.submit(), () => {}, confirm.defaultSettings);
}

var controller = {
    init: function () {
        var selector = ".js-group-subscribe ._unsubscribe";
        var unsubscribeForm = $(selector);
        unsubscribeForm.on('click', e => unsubscribeEventHanlder(e, "ti", "body"));
    }
}

export default controller;
