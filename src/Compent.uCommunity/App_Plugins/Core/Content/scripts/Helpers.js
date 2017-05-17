var Quill = require('quill');
var Delta = require('quill-delta');
var Flatpickr = require('flatpickr');
var FlatpickrLang = require('flatpickr/dist/l10n/da');

require('flatpickr/dist/flatpickr.min.css');
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
    initActivityDescription: function (holder, dataStorageElement, descId, btnElement) {
        var dataStorage = holder.find(dataStorageElement);
        var descriptionElem = holder.find(descId);
        var btn = holder.find(btnElement);

        var editor = this.initQuill(descriptionElem[0], dataStorage[0], { theme: 'snow' });

        editor.on('text-change', function () {
            if (editor.getLength() > 1 && descriptionElem.hasClass('input-validation-error')) {
                descriptionElem.removeClass('input-validation-error');
            }
        });

        btn.click(function () {
            editor.getLength() <= 1 ?
                descriptionElem.addClass('input-validation-error') :
                descriptionElem.removeClass('input-validation-error');
        });
    },
    initDatePicker: function (holder, dateElemSelector, valueSelector) {
        var dateElem = holder.find(dateElemSelector);
        var dateFormat = dateElem.data('dateFormat');
        var dateElemValue = holder.find(valueSelector);
        var defaultDate = new Date(dateElem.data('defaultDate'));

        var datePicker = new Flatpickr(dateElem[0], {
            enableTime: true,
            time_24hr: true,
            allowInput: false,
            weekNumbers: true,
            dateFormat: dateFormat,
            locale: FlatpickrLang.da,
            onChange: function (selectedDates) {
                if (selectedDates.length === 0) {
                    dateElemValue.val('');
                    return;
                }

                var selectedDate = selectedDates[0].toISOString();
                dateElemValue.val(selectedDate);
            }
        });

        defaultDate = helpers.removeOffset(defaultDate);
        datePicker.setDate(defaultDate, true);
        var minDate = new Date();
        if (defaultDate < minDate) {
            minDate = defaultDate;
        }

        datePicker.set('minDate', minDate.setHours(0));
        return datePicker;
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
    serialize: function (form) {
        if (typeof form != "object" || form.nodeName !== "FORM") return "";
        var s = [];

        for (var i = 0; i < form.elements.length; i++) {
            var field = form.elements[i];
            if (!field.name
                || field.disabled
                || field.type === "file"
                || field.type === "reset"
                || field.type === "submit"
                || field.type === "button")
                continue;

            if (field.type === "select-multiple") {
                for (var j = 0; j < field.options.length; j++) {
                    var option = field.options[j];
                    if (!option.selected) continue;
                    s[s.length] = encodeURIComponent(field.name) + "=" + encodeURIComponent(option.value);
                }
            } else if ((field.type !== "checkbox" && field.type !== "radio") || field.checked) {
                s[s.length] = encodeURIComponent(field.name) + "=" + encodeURIComponent(field.value);
            }
        }


        return s.join("&").replace(/%20/g, "+");
    }
}

export default helpers;