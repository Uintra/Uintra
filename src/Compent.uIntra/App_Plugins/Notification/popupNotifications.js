var alertify = require('alertifyjs');

function show() {
    var toasts = $(".js-toast-messages .js-toast");

    toasts.each(function (i, el) {
        let $this = $(el);
        let msg = $this.data('message');
        alertify
            .alert(msg, function () { });
    });
}

export default show;