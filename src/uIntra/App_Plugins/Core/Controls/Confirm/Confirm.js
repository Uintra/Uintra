var Alertify = require('alertifyjs/build/alertify.min');
require('alertifyjs/build/css/alertify.min.css');
require('alertifyjs/build/css/themes/default.min.css');

(function () {
    Alertify.defaults.glossary.yes = 'yes';
    Alertify.defaults.glossary.no = 'no';
    Alertify.defaults.theme.cancel = 'ajs-cancel';
    Alertify.defaults.theme.ok = 'ajs-ok';
    Alertify.defaults.theme.no = 'ajs-no';

    Alertify.dialog('customDialog', function factory() {
        return {
            setup: function () {
                return {
                    buttons: [{ text: Alertify.defaults.glossary.yes, key: 13, scope: 'auxiliary', className: Alertify.defaults.theme.ok }, { text: Alertify.defaults.glossary.no, scope: 'auxiliary', className: Alertify.defaults.theme.no }, { text: Alertify.defaults.glossary.cancel, key: 27, scope: 'auxiliary', className: Alertify.defaults.theme.cancel }],
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
        modal: true
    },

    showConfirm: function (title, text, firstCallback, secondCallback,settings) {
        Alertify.confirm(
                title,
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

    showDialog: function (title, text, callbacks, settings) {
        var callbackWrapper = function (closeEvent) {
            var callback = callbacks[closeEvent.index];
            if (callback) {
                callback();
                return true;

            }
            return false;
        }
        Alertify.customDialog(title, text, callbackWrapper).set(settings);
    },
    alert: function (title, text, callback, settings = this.defaultSettings) {
        Alertify.alert(title, text, callback).set(settings);
    }
}

export default Confirm;
