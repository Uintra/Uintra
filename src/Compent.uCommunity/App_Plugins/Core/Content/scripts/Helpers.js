var Quill = require('quill');
var Delta = require('quill-delta');

require('quill/dist/quill.core.css');
require('quill/dist/quill.bubble.css');
require('quill/dist/quill.snow.css');

var easeInOutQuad = function (t, b, c, d) {
    t /= d / 2;
    if (t < 1) return c / 2 * t * t + b;
    t--;
    return -c / 2 * (t * (t - 2) - 1) + b;
};

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
    removeOffset: function (date) {
        var dateOffset = date.getTimezoneOffset() * 60000; // [min*60000 = ms]
        return new Date(date.getTime() + dateOffset);
    },
    infiniteScrollFactory: function (onScroll) {
        return function () {
            var lock = false;
            var win = $(window);
            var doc = $(document);
            var unlock = function () { lock = false; }
            win.scroll(function () {
                if ((win.scrollTop() + 70) >= doc.height() - win.height()) {
                    if (!lock) {
                        lock = true;
                        onScroll(unlock);
                    }
                }
            });
        }
    },
    scrollTo: function (element, to, duration) {
        var start = element.scrollTop,
           change = to - start,
           currentTime = 0,
           increment = 20;

        var animateScroll = function () {
            currentTime += increment;
            var val = easeInOutQuad(currentTime, start, change, duration);
            element.scrollTop = val;
            if (currentTime < duration) {
                setTimeout(animateScroll, increment);
            }
        };

        animateScroll();
    },
    localStorage: {
        getItem: function (key) {
            return JSON.parse(localStorage.getItem(key));
        },
        setItem: function (key, obj) {
            localStorage.setItem(key, JSON.stringify(obj));
        },
        removeItem: function (key) {
            localStorage.removeItem(key);
        }
    },
}

export default helpers;