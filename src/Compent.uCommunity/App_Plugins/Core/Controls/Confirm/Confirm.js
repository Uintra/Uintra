var Alertify = require('alertifyjs/build/alertify.min');

(function () {
    Alertify.defaults.glossary.yes = 'yes';
    Alertify.defaults.glossary.no = 'no';
    Alertify.defaults.theme.cancel = 'ajs-cancel';

    Alertify.dialog('customDialog', function factory() {
        return {
            setup: function () {
                return {
                    buttons: [{ text: Alertify.defaults.glossary.yes, key: 13, scope: 'auxiliary' }, { text: Alertify.defaults.glossary.no, scope: 'auxiliary' }, { text: Alertify.defaults.glossary.cancel, key: 27, scope: 'auxiliary', className: Alertify.defaults.theme.cancel }],
                    options: {
                        modal: true
                    }
                };
            }
        }
    }, false, 'alert');   
})();

var Confirm = {
    defaultSettings: {
        transition: 'zoom',
        maximizable: false,
        pinnable: false,
        movable: false,
        resizable: false,
        closable: true,
        modal: true,
        title: "<h2 class='ajs-title'>Confirm</h1>"
    },

    showConfirm: function (text, firstCallback, secondCallback,settings) {
        Alertify.confirm('',
                text,
                function () {
                    firstCallback();
                },
                function () {
                    if (secondCallback) {
                        secondCallback();
                    }
                })
            .set(settings);
    },

    showDialog: function (text, callbacks,settings) {
        var callbackWrapper = function (closeEvent) {
            var callback = callbacks[closeEvent.index];
            if (callback) {
                callback();
                return true;

            }
            return false;
        }
        Alertify.customDialog('', text, callbackWrapper).set(settings);
    }
}

export default Confirm;
