var Quill = require('quill');
var Delta = require('quill-delta');
var Dotdotdot = require('dotdotdot');
var Flatpickr = require('flatpickr');
require('simple-scrollbar');

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
            if (!quill.getText().trim()) {
                dataStorage.value = '';
                return;
            }

            dataStorage.value = quill.container.firstChild.innerHTML;
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
        var dateParentNode = dateElem.parent();
        var dateFormat = dateElem.data('dateFormat');
        var dateElemValue = holder.find(valueSelector);
        var defaultDate = new Date(dateElem.data('defaultDate'));
        var enableTime = dateElem.data('enableTime');
        var closeButton = document.createElement("span");
        var clearButton = dateParentNode.find('.js-clear-date');
        closeButton.className = "flatpickr__close";
        closeButton.addEventListener("click", function () {
            datePicker.close();
        });

        var datePicker = new Flatpickr(dateElem[0], {
            enableTime: enableTime != null ? enableTime : true,
            time_24hr: true,
            allowInput: false,
            weekNumbers: true,
            dateFormat: dateFormat,
            minuteIncrement: 1,
            onChange: function (selectedDates) {
                if (selectedDates.length === 0) {
                    dateElemValue.val('');
                    return;
                }

                var selectedDate = selectedDates[0].toISOString();
                dateElemValue.val(selectedDate);
                clearButton.removeClass("hide");
            }
        });

        datePicker.calendarContainer.appendChild(closeButton);

        datePicker.setDate(defaultDate, true);
        var minDate = new Date();
        if (defaultDate < minDate) {
            minDate = defaultDate;
        }

        datePicker.set('minDate', minDate.setHours(0));
        if(datePicker.selectedDates.length > 0){
            clearButton.removeClass("hide");
        };

        clearButton.click(function () {
            datePicker.clear();
            $(this).addClass("hide");
        });

        return datePicker;
    },
    infiniteScrollFactory: function (onScroll, scrollContainer) {
        let settings = {
            defaultScrollKoef: 150,
            storageName: 'infiniteScroll',
            loaderSelector: '.js-loader',
            $container: null,
            reload: null
        }

        let lock = false;
        let win = $(window);
        let doc = $(document);

        $.extend(settings, options);

        const showLoadingStatus = function () {
            $(settings.loaderSelector).show();
        }

        const hideLoadingStatus = function () {
            $(settings.loaderSelector).hide();
        }

        const reloadData = function () {
            showLoadingStatus();
            helpers.state.save(settings.storageName);
            var promise = settings.reload();
            promise.then(unlock, unlock)
            return promise;
        }

        const scrollPrevented = function () {
            return !!parseInt(settings.$container.find('input[name="preventScrolling"]').val()) | false;
        }

        const unlock = function () {
            lock = false;
            hideLoadingStatus();
        }

        const loadNextPage = function () {
            if (!lock) {
                lock = true;

                if (scrollPrevented()) {
                    unlock();
                } else {
                    helpers.state.page++;
                    reloadData()
                }
            }
        }

        win.on('scroll.infinite', function () {
            if (settings.$container && settings.$container.length > 0) {
                let containerRect = settings.$container.get(0).getBoundingClientRect();

                if (-containerRect.top + settings.defaultScrollKoef >= containerRect.height - screen.height) {
                    loadNextPage();
                }
            } else {
                if ((win.scrollTop() + settings.defaultScrollKoef) >= doc.height() - win.height()) {
                    loadNextPage();
                }
            }
        });

        //if we don't have scroll load more items
        if (document.body.scrollHeight == document.body.clientHeight) {
            loadNextPage();
        }

        helpers.state.restoreState(reloadData, settings.storageName);
    },
    scrollTo: function (element, to, duration) {
        var start = element.scrollTop,
            change = to - start,
            currentTime = 0,
            increment = 50;

        var animateScroll = function () {
            currentTime += increment;
            var val = easeInOutQuad(currentTime, start, change, duration);
            top.window.scroll(0, val)
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
            else if (field.type === "checkbox" ) {
                s[s.length] = encodeURIComponent(field.name) + "=" + encodeURIComponent(field.checked);
            }
        }


        return s.join("&").replace(/%20/g, "+");
    },
    clampText: function(container, url) {
        var $container = $(container);
        $container.dotdotdot({
            watch: 'window'
        });
        $container.contents().wrap( "<a href='" + url +"' class='feed__item-txt-link'></a>" );
    },
    initScrollbar: function(el){
        SimpleScrollbar.initEl(el);
    },
    state: {
        get page() {
            return document.querySelector('input[name="page"]').value || 1;
        },
        set page(val) {
            document.querySelector('input[name="page"]').value = val;
        },
        save(storageName) {
            helpers.localStorage.setItem(storageName, { page: this.page });
        },
        restoreState(reloadPromise, storageName) {
            const hash = (window.location.hash || '').replace('#', '');

            if (hash) {
                let savedState = helpers.localStorage.getItem(storageName);

                helpers.state.page = (savedState || {}).page || 1;

                reloadPromise().then(function () {
                    let elem = document.querySelector('[data-anchor="' + hash + '"]');

                    if (elem) {
                        helpers.scrollTo(document.body, elem.offsetTop, 300);
                        window.history.pushState('', document.title, window.location.pathname);
                    }
                });
            } else {
                helpers.localStorage.removeItem(storageName);
            }
        }
    }
}

export default helpers;