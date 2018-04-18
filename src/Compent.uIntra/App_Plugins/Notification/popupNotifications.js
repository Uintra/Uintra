import confirm from "./../Core/Controls/Confirm/Confirm";
import ajax from "./../Core/Content/scripts/Ajax";

let alertify = require('alertifyjs/build/alertify.min');

function show() {
    const messagesHolder = document.querySelector('.js-popup-messages');
    const toasts = [...messagesHolder.querySelectorAll('.js-popup')];
    if (!toasts.length) return;

    var url = messagesHolder.dataset['viewPopupUrl'];

    const okBtnText = messagesHolder.dataset['okBtnText'];
    alertify.defaults.glossary.ok = okBtnText;

    toasts.forEach((el) => {
        let msg = el.dataset['message'];

        confirm.alert(
            '',
            msg,
            () => {
                let data = { id: el.dataset['id'] };
                ajax.post(url, data);
            },
            confirm.defaultSettings);
    });
}

export default show;