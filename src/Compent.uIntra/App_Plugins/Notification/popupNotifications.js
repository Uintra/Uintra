import confirm from "./../Core/Controls/Confirm/Confirm";
import ajax from "./../Core/Content/scripts/Ajax";
import { debug } from "util";


function show() {
    var toasts = $(".js-popup-messages .js-popup");

    toasts.each(function (i, el) {
        let $this = $(el);
        let msg = $this.data('message');

        confirm.showConfirm(
                ' ',
                msg,
                () => {
                    let data = { id: $this.data("id") };
                    ajax.post('/umbraco/surface/Notification/ViewPopup/', data);
                },
                () => {},
                confirm.defaultSettings);});
}

export default show;