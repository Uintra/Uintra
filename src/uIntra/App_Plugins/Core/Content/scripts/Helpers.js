const Quill = require('quill');
const Delta = require('quill-delta');
const Flatpickr = require('flatpickr');
import ajax from './Ajax';

require('simple-scrollbar');
require('flatpickr/dist/flatpickr.min.css');
require('quill/dist/quill.snow.css');

var urlDetectRegexes = [];

ajax.get('/umbraco/api/LinkPreview/config')
    .then(function (response) {
        var regexes = response.data.urlRegex.map(r => new RegExp(r));
        urlDetectRegexes = regexes;
    });

function matchUponMultiple(regexes, text) {
    return regexes.map(regex => text.match(regex)).find(m => m !== null || m !== undefined || m !== {});
}

const easeInOutQuad = function (t, b, c, d) {
    t /= d / 2;
    if (t < 1) return c / 2 * t * t + b;
    t--;
    return -c / 2 * (t * (t - 2) - 1) + b;
};

const helpers = {
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

        let settings = {
            theme: 'snow'
        }

        if (typeof options == 'undefined') {
            settings.modules = {
                toolbar: {
                    container: [['emoji'], ['bold', 'italic', 'underline'], ['link']]
                }
            };
        }
        else {
            $.extend(settings, options);
        }

        let quill = new Quill(source, settings);
        let toolbar = quill.getModule('toolbar');

        //override default link handler
        toolbar.addHandler('link', function (value) {
            if (value) {
                let range = this.quill.getSelection();
                if (range == null || range.length == 0) return;
                let preview = this.quill.getText(range);
                let tooltip = this.quill.theme.tooltip;
                if (/^\S+@\S+\.\S+$/.test(preview) && preview.indexOf('mailto:') !== 0) {
                    preview = 'mailto:' + preview;
                    tooltip.edit('link', preview);
                } else {
                    tooltip.edit('link', '');
                }
            } else {
                this.quill.format('link', false);
            }
        });

        let onLinkDetectedCallbacks = [];

        quill.onLinkDetected = function (cb) {
            onLinkDetectedCallbacks.push(cb);
        }

        function triggerLinkDetectedEvent(link) {
            onLinkDetectedCallbacks.forEach((cb) => cb(link));
        }

        quill.on('text-change', (delta, oldDelta, source) => {
            var n = quill.container.querySelectorAll("img").length;
            if (!quill.getText().trim() && n < 1) {
                dataStorage.value = '';
                return;
            }
            dataStorage.value = quill.container.firstChild.innerHTML;

            if (delta.ops.length === 2 && delta.ops[0].retain && isWhitespace(delta.ops[1].insert)) {
                var endRetain = delta.ops[0].retain;
                var text = quill.getText().substr(0, endRetain);
                var matches = matchUponMultiple(urlDetectRegexes, text);

                if (matches) {
                    var url = matches[0];
                    triggerLinkDetectedEvent(url);

                    var ops = [];
                    if (endRetain > url.length) {
                        ops.push({ retain: endRetain - url.length });
                    }

                    ops = ops.concat([
                        { delete: url.length },
                        { insert: url, attributes: { link: url } }
                    ]);

                    var selectionBeforeUpdate = quill.getSelection();

                    quill.updateContents({
                        ops: ops
                    });

                    quill.setSelection(selectionBeforeUpdate);
                }
            }

            function isWhitespace(ch) {
                var whiteSpace = false
                if ((ch == ' ') || (ch == '\t') || (ch == '\n')) {
                    whiteSpace = true;
                }
                return whiteSpace;
            }
        });

        quill.clipboard.addMatcher(Node.TEXT_NODE, function (node, delta) {
            if (typeof (node.data) !== 'string') return delta;
            var text = node.data;

            var matches = matchUponMultiple(urlDetectRegexes, text);

            if (matches && matches.length > 0) {
                var ops = [];
                var match = matches[0];
                triggerLinkDetectedEvent(match);
                var split = text.split(match);
                var beforeLink = split.shift();
                ops.push({ insert: beforeLink });
                ops.push({ insert: match, attributes: { link: match } });
                text = split.join(match);

                ops.push({ insert: text });
                delta.ops = ops;
            }

            return delta;
        });

        //init emoji smiles
        helpers.initSmiles(quill, toolbar.container);

        //override link's tooltip placeholder
        const tooltip = quill.theme.tooltip;
        let input = tooltip.root.querySelector("input[data-link]");
        input.dataset.link = location.host;

        return quill;
    },
    initSmiles: function (container, toolbar, index) {
        const emoji = {
            "smile": {
                "shortcode": ":)",
                "translation": "Smile"
            },
            "sad": {
                "shortcode": ":(",
                "translation": "Sad"
            },
            "wink": {
                "shortcode": ";)",
                "translation": "Wink"
            },
            "shocked": {
                "shortcode": ":|",
                "translation": "Shocked"
            },
            "tease": {
                "shortcode": ":p",
                "translation": "Tease"
            },

            "funny": {
                "shortcode": ":D",
                "translation": "Funny"
            },
            "angry": {
                "shortcode": ":<",
                "translation": "Angry"
            },
            "skeptical": {
                "shortcode": ":^)",
                "translation": "Skeptical"
            },
            "surprised": {
                "shortcode": ":o",
                "translation": "Surprised"
            },
            "great": {
                "shortcode": ":+1",
                "translation": "Great"
            },

            "joy": {
                "shortcode": ":-)",
                "translation": "Joy"
            },
            "love": {
                "shortcode": ":x",
                "translation": "Love"
            },
            "party": {
                "shortcode": "<o)",
                "translation": "Party"
            },
            "fever": {
                "shortcode": "fever",
                "translation": "Fever"
            },
            "sleepy": {
                "shortcode": "|-)",
                "translation": "Sleepy"
            }
        };
        const body = document.querySelector('body');
        const path = '/App_Plugins/Core/Content/styles/emoji-data/';
        let emojiContainer;
        let emojiList;
        let emojiListItem;
        let emojiListImage;
        let emojiBtn = toolbar.querySelector(".ql-emoji");

        emojiContainer = document.createElement("div");
        emojiContainer.classList.add("js-emoji");
        emojiContainer.classList.add("emoji");
        emojiContainer.classList.add("hidden");

        emojiList = document.createElement("ul");
        emojiList.classList.add("emoji__list");

        for (var i in emoji) {
            emojiListItem = document.createElement("li");
            emojiListItem.classList.add("emoji__list-item");

            emojiListItem.addEventListener('click', function (event) {
                CopyClipboard(getHTML(event.target));
                emojiContainer.classList.add("hidden");
            });

            emojiListImage = document.createElement("img");
            emojiListImage.setAttribute("src", path + i + ".svg");
            emojiListImage.setAttribute("title", emoji[i].translation + " (" + emoji[i].shortcode + ")");
            emojiListImage.setAttribute("width", "20");
            emojiListImage.setAttribute("height", "20");
            emojiListImage.classList.add("emoji-icon");
            emojiListImage.classList.add(i);

            emojiListItem.appendChild(emojiListImage);
            emojiList.appendChild(emojiListItem);
        }

        emojiContainer.appendChild(emojiList);
        emojiBtn.insertAdjacentElement('afterend', emojiContainer);

        emojiBtn.addEventListener('click', function () {
            if (emojiContainer.classList.contains("hidden")) {
                emojiContainer.classList.remove("hidden");

                const eRect = emojiContainer.getBoundingClientRect();
                if (eRect.bottom >= document.body.clientHeight) {
                    emojiContainer.classList.add('_in-top');
                }
            }
            else {
                emojiContainer.classList.add("hidden");
            }
        });

        container.on('text-change', function (eventName, ...args) {
            index = getIndex();
            var text = container.getText();
            for (var i in emoji) {
                if (text.indexOf(emoji[i].shortcode) >= 0) {
                    var n = container.container.querySelectorAll("img").length;
                    var index = text.indexOf(emoji[i].shortcode) + n;
                    container.updateContents(new Delta()
                        .retain(index)
                        .delete(emoji[i].shortcode.length)
                    );
                    container.insertEmbed(index, 'image', path + i + ".svg");
                    container.formatText(index, 1, 'width', '20px');
                    container.setSelection(++index);
                    break;
                }
            }
        });

        body.addEventListener("click", function (ev) {
            isOutsideClick(emojiContainer, ev.target, function () {
                emojiContainer.classList.add("hidden");
            });
        });

        function CopyClipboard(target, index) {
            if (!index) {
                index = getIndex();
            }
            container.clipboard.dangerouslyPasteHTML(index, target);
            container.setSelection(++index);
        }

        function getHTML(el) {
            if (!el || !el.tagName) return '';
            var txt,
                clone = document.createElement("div");

            clone.appendChild(el.cloneNode(false));
            txt = clone.innerHTML;
            clone = null;
            return txt;
        }

        function getIndex() {
            let range = container.getSelection();
            let index;
            if (range) {
                if (range.length == 0) {
                    index = range.index;
                } else {
                    index = range.index + range.length;
                }
            } else {
                index = 0;
            }
            return index;
        }

        function isOutsideClick(el, target, callback) {
            if (el && !el.contains(target) && target != emojiBtn) {
                if (typeof callback === "function") {
                    callback();
                }
            }
        };
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
        if (datePicker.selectedDates.length > 0) {
            clearButton.removeClass("hide");
        };

        clearButton.click(function () {
            datePicker.clear();
            $(this).addClass("hide");
        });

        return datePicker;
    },
    infiniteScrollFactory: function (options) {
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
            else if (field.type === "checkbox") {
                s[s.length] = encodeURIComponent(field.name) + "=" + encodeURIComponent(field.checked);
            }
        }


        return s.join("&").replace(/%20/g, "+");
    },
    clampText: function (container, url) {
        var $container = $(container);
        $container.dotdotdot({
            watch: 'window'
        });
        $container.contents().wrap("<a href='" + url + "' class='feed__item-txt-link'></a>");
    },
    initScrollbar: function (el) {
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