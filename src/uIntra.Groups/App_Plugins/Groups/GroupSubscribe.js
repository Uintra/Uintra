import confirm from './../Core/Controls/Confirm/Confirm';

function unsubscribeEventHanlder(e) {
    e.preventDefault();
    var title = e.target.dataset.title;
    var message = e.target.dataset.message;
    confirm.showConfirm(title, message, () => e.target.form.submit(), () => {}, confirm.defaultSettings);
}

var controller = {
    init: function () {
        var selector = ".js-group-subscribe ._unsubscribe";
        var unsubscribeForm = $(selector);
        unsubscribeForm.on('click', e => unsubscribeEventHanlder(e));
    }
}

export default controller;
