import confirm from "./../Core/Controls/Confirm/Confirm";
import ajax from "./../Core/Content/scripts/Ajax";

let alertify = require('alertifyjs/build/alertify.min');
const popupHiddenFieldClass = 'js-popup-hidden-field';
let viewPopupUrl;

function show() {
    const popupsHolder = document.querySelector('.js-popups-holder');
    if (!popupsHolder) return;

    const popups = [...popupsHolder.querySelectorAll('.js-popup')];
    if (!popups.length) return;

    viewPopupUrl = popupsHolder.dataset['viewPopupUrl'];

    const okBtnText = popupsHolder.dataset['okBtnText'];
    alertify.defaults.glossary.ok = okBtnText;

    confirm.defaultSettings.onfocus = popupFocusHandler;

    popups.forEach((popup) => {
        let notificationId = popup.dataset['id'];
        let message = popup.dataset['message'] + '<input type="hidden" class="' + popupHiddenFieldClass + '" value="' + notificationId + '">';

        confirm.alert(
            '',
            message,
            () => { viewPopup(notificationId); },
            confirm.defaultSettings);
    });
}

function popupFocusHandler() {
    const hiddenFields = [...document.querySelectorAll("." + popupHiddenFieldClass)];
    hiddenFields.forEach((el) => {
        let notificationId = el.value;
        let link = el.parentElement.querySelector("a");

        link.addEventListener('click', (e) => {
            e.preventDefault();
            viewPopup(notificationId);
            window.location = link.href;
        });
    });
}

function viewPopup(notificationId) {
    ajax.post(viewPopupUrl, { id: notificationId });
}

export default show;