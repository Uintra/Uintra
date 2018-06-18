import confirm from "../../Controls/Confirm/Confirm";
import alertify from 'alertifyjs/build/alertify.min';
require('alertifyjs/build/css/alertify.min.css');
require('alertifyjs/build/css/themes/default.min.css');

const def = {
    text: "Are you sure?",
    ok: "Yes",
    no: "No"
}
var noop = () => { };

function showConfirm(link) {
    let text = link.dataset['text'] || def.text;
    let ok = link.dataset['ok'] || def.ok;
    let no = link.dataset['cancel'] || def.no;

    alertify.defaults.glossary.cancel = no;
    alertify.defaults.glossary.ok = ok;

    confirm.showConfirm('', text, () => {
        link.confirmed = true;
        link.click();
    }, noop, confirm.defaultSettings);
}

function initConfirmForActionLink() {
    var actionLinks = [...document.querySelectorAll('.js-action-with-confirm')];
    if (!actionLinks.length) return;

    actionLinks.forEach((el) => {
        el.addEventListener('click', (e) => {
            if (el.confirmed) return;
            e.preventDefault();
            e.stopPropagation();
            showConfirm(el);
        });
    });
}

export default initConfirmForActionLink;