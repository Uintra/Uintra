var helpers = {
    deepClone: function (obj) {
        return JSON.parse(JSON.stringify(obj));
    },
    initQuill: function (source, dataStorage, options) {
        if (!dataStorage) {
            throw new Error("Hided input field missing");
        }

        if (!source) {
            throw new Error("Source field missing");
        }

        var quill = new Quill(source, options);

        quill.on('text-change', (delta, oldDelta, source) => {
            var text = quill.container.firstChild.innerHTML;
            if (text.replace(/(<([^>]+)>)/ig, '').replace('<br>', '').length === 0) {
                dataStorage.value = '';
                return;
            }
            dataStorage.value = text;
        });

        quill.clipboard.addMatcher(Node.ELEMENT_NODE, function (node, delta) {
            var plaintext = $.trim($(node).text());
            return new Delta().insert(plaintext);
        });

        return quill;
    },
    removeOffset: function(date) {
        var dateOffset = date.getTimezoneOffset() * 60000; // [min*60000 = ms]
        return new Date(date.getTime() + dateOffset);
    }
}

window.App = window.App || {};
window.App.Helpers = helpers;