// Production steps of ECMA-262, Edition 6, 22.1.2.1
if (!Array.from) {
    Array.from = (function () {
        var toStr = Object.prototype.toString;
        var isCallable = function (fn) {
            return typeof fn === 'function' || toStr.call(fn) === '[object Function]';
        };
        var toInteger = function (value) {
            var number = Number(value);
            if (isNaN(number)) { return 0; }
            if (number === 0 || !isFinite(number)) { return number; }
            return (number > 0 ? 1 : -1) * Math.floor(Math.abs(number));
        };
        var maxSafeInteger = Math.pow(2, 53) - 1;
        var toLength = function (value) {
            var len = toInteger(value);
            return Math.min(Math.max(len, 0), maxSafeInteger);
        };

        // The length property of the from method is 1.
        return function from(arrayLike/*, mapFn, thisArg */) {
            // 1. Let C be the this value.
            var C = this;

            // 2. Let items be ToObject(arrayLike).
            var items = Object(arrayLike);

            // 3. ReturnIfAbrupt(items).
            if (arrayLike == null) {
                throw new TypeError('Array.from requires an array-like object - not null or undefined');
            }

            // 4. If mapfn is undefined, then let mapping be false.
            var mapFn = arguments.length > 1 ? arguments[1] : void undefined;
            var T;
            if (typeof mapFn !== 'undefined') {
                // 5. else
                // 5. a If IsCallable(mapfn) is false, throw a TypeError exception.
                if (!isCallable(mapFn)) {
                    throw new TypeError('Array.from: when provided, the second argument must be a function');
                }

                // 5. b. If thisArg was supplied, let T be thisArg; else let T be undefined.
                if (arguments.length > 2) {
                    T = arguments[2];
                }
            }

            // 10. Let lenValue be Get(items, "length").
            // 11. Let len be ToLength(lenValue).
            var len = toLength(items.length);

            // 13. If IsConstructor(C) is true, then
            // 13. a. Let A be the result of calling the [[Construct]] internal method 
            // of C with an argument list containing the single item len.
            // 14. a. Else, Let A be ArrayCreate(len).
            var A = isCallable(C) ? Object(new C(len)) : new Array(len);

            // 16. Let k be 0.
            var k = 0;
            // 17. Repeat, while k < len… (also steps a - h)
            var kValue;
            while (k < len) {
                kValue = items[k];
                if (mapFn) {
                    A[k] = typeof T === 'undefined' ? mapFn(kValue, k) : mapFn.call(T, kValue, k);
                } else {
                    A[k] = kValue;
                }
                k += 1;
            }
            // 18. Let putStatus be Put(A, "length", len, true).
            A.length = len;
            // 20. Return A.
            return A;
        };
    }());
}

if (window.Element) {
    Element.prototype.findAncestorByClassName = function (className) {
        var el = this;
        while ((el = el.parentElement) && !el.classList.contains(className));
        return el;
    };
}

(function () {

    if (typeof window.CustomEvent === "function") return false;

    function CustomEvent(event, params) {
        params = params || { bubbles: false, cancelable: false, detail: undefined };
        var evt = document.createEvent('CustomEvent');
        evt.initCustomEvent(event, params.bubbles, params.cancelable, params.detail);
        return evt;
    }

    CustomEvent.prototype = window.Event.prototype;

    window.CustomEvent = CustomEvent;
})();

// https://tc39.github.io/ecma262/#sec-array.prototype.includes
if (!Array.prototype.includes) {
    Object.defineProperty(Array.prototype, 'includes', {
        value: function (searchElement, fromIndex) {

            // 1. Let O be ? ToObject(this value).
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If len is 0, return false.
            if (len === 0) {
                return false;
            }

            // 4. Let n be ? ToInteger(fromIndex).
            //    (If fromIndex is undefined, this step produces the value 0.)
            var n = fromIndex | 0;

            // 5. If n ≥ 0, then
            //  a. Let k be n.
            // 6. Else n < 0,
            //  a. Let k be len + n.
            //  b. If k < 0, let k be 0.
            var k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);

            function sameValueZero(x, y) {
                return x === y || (typeof x === 'number' && typeof y === 'number' && isNaN(x) && isNaN(y));
            }

            // 7. Repeat, while k < len
            while (k < len) {
                // a. Let elementK be the result of ? Get(O, ! ToString(k)).
                // b. If SameValueZero(searchElement, elementK) is true, return true.
                // c. Increase k by 1. 
                if (sameValueZero(o[k], searchElement)) {
                    return true;
                }
                k++;
            }

            // 8. Return false
            return false;
        }
    });
}

/**
 * Difference;
 * Returns the elements of the first array that are not in the second array
 */
Array.prototype.except = function (a) {
    return this.filter(el => a.indexOf(el) === -1);
};

/**
 * NodeList forEach
 */

if (window.NodeList && !NodeList.prototype.forEach) {
    NodeList.prototype.forEach = function (callback, thisArg) {
        thisArg = thisArg || window;
        for (var i = 0; i < this.length; i++) {
            callback.call(thisArg, this[i], i, this);
        }
    };
}

(function (ElementProto) {
    if (typeof ElementProto.matches !== 'function') {
        ElementProto.matches = ElementProto.msMatchesSelector || ElementProto.mozMatchesSelector || ElementProto.webkitMatchesSelector || function matches(selector) {
            var element = this;
            var elements = (element.document || element.ownerDocument).querySelectorAll(selector);
            var index = 0;

            while (elements[index] && elements[index] !== element) {
                ++index;
            }

            return Boolean(elements[index]);
        };
    }

    if (typeof ElementProto.closest !== 'function') {
        ElementProto.closest = function closest(selector) {
            var element = this;

            while (element && element.nodeType === 1) {
                if (element.matches(selector)) {
                    return element;
                }

                element = element.parentNode;
            }

            return null;
        };
    }

    if (typeof Object.assign != 'function') {
        Object.assign = function (target) {
            'use strict';
            if (target == null) {
                throw new TypeError('Cannot convert undefined or null to object');
            }

            target = Object(target);
            for (var index = 1; index < arguments.length; index++) {
                var source = arguments[index];
                if (source != null) {
                    for (var key in source) {
                        if (Object.prototype.hasOwnProperty.call(source, key)) {
                            target[key] = source[key];
                        }
                    }
                }
            }
            return target;
        };
    }

    Number.isNaN = Number.isNaN || function (value) {
        return value !== value;
    }

})(window.Element.prototype);

(function (global) {
    /**
     * Polyfill URLSearchParams
     *
     * Inspired from : https://github.com/WebReflection/url-search-params/blob/master/src/url-search-params.js
     */

    var checkIfIteratorIsSupported = function () {
        try {
            return !!Symbol.iterator;
        } catch (error) {
            return false;
        }
    };


    var iteratorSupported = checkIfIteratorIsSupported();

    var createIterator = function (items) {
        var iterator = {
            next: function () {
                var value = items.shift();
                return { done: value === void 0, value: value };
            }
        };

        if (iteratorSupported) {
            iterator[Symbol.iterator] = function () {
                return iterator;
            };
        }

        return iterator;
    };

    /**
     * Search param name and values should be encoded according to https://url.spec.whatwg.org/#urlencoded-serializing
     * encodeURIComponent() produces the same result except encoding spaces as `%20` instead of `+`.
     */
    var serializeParam = function (value) {
        return encodeURIComponent(value).replace(/%20/g, '+');
    };

    var deserializeParam = function (value) {
        return decodeURIComponent(value).replace(/\+/g, ' ');
    };

    var polyfillURLSearchParams = function () {

        var URLSearchParams = function (searchString) {
            Object.defineProperty(this, '_entries', { writable: true, value: {} });
            var typeofSearchString = typeof searchString;

            if (typeofSearchString === 'undefined') {
                // do nothing
            } else if (typeofSearchString === 'string') {
                if (searchString !== '') {
                    this._fromString(searchString);
                }
            } else if (searchString instanceof URLSearchParams) {
                var _this = this;
                searchString.forEach(function (value, name) {
                    _this.append(name, value);
                });
            } else if ((searchString !== null) && (typeofSearchString === 'object')) {
                if (Object.prototype.toString.call(searchString) === '[object Array]') {
                    for (var i = 0; i < searchString.length; i++) {
                        var entry = searchString[i];
                        if ((Object.prototype.toString.call(entry) === '[object Array]') || (entry.length !== 2)) {
                            this.append(entry[0], entry[1]);
                        } else {
                            throw new TypeError('Expected [string, any] as entry at index ' + i + ' of URLSearchParams\'s input');
                        }
                    }
                } else {
                    for (var key in searchString) {
                        if (searchString.hasOwnProperty(key)) {
                            this.append(key, searchString[key]);
                        }
                    }
                }
            } else {
                throw new TypeError('Unsupported input\'s type for URLSearchParams');
            }
        };

        var proto = URLSearchParams.prototype;

        proto.append = function (name, value) {
            if (name in this._entries) {
                this._entries[name].push(String(value));
            } else {
                this._entries[name] = [String(value)];
            }
        };

        proto.delete = function (name) {
            delete this._entries[name];
        };

        proto.get = function (name) {
            return (name in this._entries) ? this._entries[name][0] : null;
        };

        proto.getAll = function (name) {
            return (name in this._entries) ? this._entries[name].slice(0) : [];
        };

        proto.has = function (name) {
            return (name in this._entries);
        };

        proto.set = function (name, value) {
            this._entries[name] = [String(value)];
        };

        proto.forEach = function (callback, thisArg) {
            var entries;
            for (var name in this._entries) {
                if (this._entries.hasOwnProperty(name)) {
                    entries = this._entries[name];
                    for (var i = 0; i < entries.length; i++) {
                        callback.call(thisArg, entries[i], name, this);
                    }
                }
            }
        };

        proto.keys = function () {
            var items = [];
            this.forEach(function (value, name) {
                items.push(name);
            });
            return createIterator(items);
        };

        proto.values = function () {
            var items = [];
            this.forEach(function (value) {
                items.push(value);
            });
            return createIterator(items);
        };

        proto.entries = function () {
            var items = [];
            this.forEach(function (value, name) {
                items.push([name, value]);
            });
            return createIterator(items);
        };

        if (iteratorSupported) {
            proto[Symbol.iterator] = proto.entries;
        }

        proto.toString = function () {
            var searchArray = [];
            this.forEach(function (value, name) {
                searchArray.push(serializeParam(name) + '=' + serializeParam(value));
            });
            return searchArray.join('&');
        };


        global.URLSearchParams = URLSearchParams;
    };

    if (!('URLSearchParams' in global) || (new URLSearchParams('?a=1').toString() !== 'a=1')) {
        polyfillURLSearchParams();
    }

    var proto = URLSearchParams.prototype;

    if (typeof proto.sort !== 'function') {
        proto.sort = function () {
            var _this = this;
            var items = [];
            this.forEach(function (value, name) {
                items.push([name, value]);
                if (!_this._entries) {
                    _this.delete(name);
                }
            });
            items.sort(function (a, b) {
                if (a[0] < b[0]) {
                    return -1;
                } else if (a[0] > b[0]) {
                    return +1;
                } else {
                    return 0;
                }
            });
            if (_this._entries) { // force reset because IE keeps keys index
                _this._entries = {};
            }
            for (var i = 0; i < items.length; i++) {
                this.append(items[i][0], items[i][1]);
            }
        };
    }

    if (typeof proto._fromString !== 'function') {
        Object.defineProperty(proto, '_fromString', {
            enumerable: false,
            configurable: false,
            writable: false,
            value: function (searchString) {
                if (this._entries) {
                    this._entries = {};
                } else {
                    var keys = [];
                    this.forEach(function (value, name) {
                        keys.push(name);
                    });
                    for (var i = 0; i < keys.length; i++) {
                        this.delete(keys[i]);
                    }
                }

                searchString = searchString.replace(/^\?/, '');
                var attributes = searchString.split('&');
                var attribute;
                for (var i = 0; i < attributes.length; i++) {
                    attribute = attributes[i].split('=');
                    this.append(
                        deserializeParam(attribute[0]),
                        (attribute.length > 1) ? deserializeParam(attribute[1]) : ''
                    );
                }
            }
        });
    }

    // HTMLAnchorElement

})(
    (typeof global !== 'undefined') ? global
        : ((typeof window !== 'undefined') ? window
            : ((typeof self !== 'undefined') ? self : this))
    );

(function (global) {
    /**
     * Polyfill URL
     *
     * Inspired from : https://github.com/arv/DOM-URL-Polyfill/blob/master/src/url.js
     */

    var checkIfURLIsSupported = function () {
        try {
            var u = new URL('b', 'http://a');
            u.pathname = 'c%20d';
            return (u.href === 'http://a/c%20d') && u.searchParams;
        } catch (e) {
            return false;
        }
    };


    var polyfillURL = function () {
        var _URL = global.URL;

        var URL = function (url, base) {
            if (typeof url !== 'string') url = String(url);

            // Only create another document if the base is different from current location.
            var doc = document, baseElement;
            if (base && (global.location === void 0 || base !== global.location.href)) {
                doc = document.implementation.createHTMLDocument('');
                baseElement = doc.createElement('base');
                baseElement.href = base;
                doc.head.appendChild(baseElement);
                try {
                    if (baseElement.href.indexOf(base) !== 0) throw new Error(baseElement.href);
                } catch (err) {
                    throw new Error('URL unable to set base ' + base + ' due to ' + err);
                }
            }

            var anchorElement = doc.createElement('a');
            anchorElement.href = url;
            if (baseElement) {
                doc.body.appendChild(anchorElement);
                anchorElement.href = anchorElement.href; // force href to refresh
            }

            if (anchorElement.protocol === ':' || !/:/.test(anchorElement.href)) {
                throw new TypeError('Invalid URL');
            }

            Object.defineProperty(this, '_anchorElement', {
                value: anchorElement
            });


            // create a linked searchParams which reflect its changes on URL
            var searchParams = new URLSearchParams(this.search);
            var enableSearchUpdate = true;
            var enableSearchParamsUpdate = true;
            var _this = this;
            ['append', 'delete', 'set'].forEach(function (methodName) {
                var method = searchParams[methodName];
                searchParams[methodName] = function () {
                    method.apply(searchParams, arguments);
                    if (enableSearchUpdate) {
                        enableSearchParamsUpdate = false;
                        _this.search = searchParams.toString();
                        enableSearchParamsUpdate = true;
                    }
                };
            });

            Object.defineProperty(this, 'searchParams', {
                value: searchParams,
                enumerable: true
            });

            var search = void 0;
            Object.defineProperty(this, '_updateSearchParams', {
                enumerable: false,
                configurable: false,
                writable: false,
                value: function () {
                    if (this.search !== search) {
                        search = this.search;
                        if (enableSearchParamsUpdate) {
                            enableSearchUpdate = false;
                            this.searchParams._fromString(this.search);
                            enableSearchUpdate = true;
                        }
                    }
                }
            });
        };

        var proto = URL.prototype;

        var linkURLWithAnchorAttribute = function (attributeName) {
            Object.defineProperty(proto, attributeName, {
                get: function () {
                    return this._anchorElement[attributeName];
                },
                set: function (value) {
                    this._anchorElement[attributeName] = value;
                },
                enumerable: true
            });
        };

        ['hash', 'host', 'hostname', 'port', 'protocol']
            .forEach(function (attributeName) {
                linkURLWithAnchorAttribute(attributeName);
            });

        Object.defineProperty(proto, 'search', {
            get: function () {
                return this._anchorElement['search'];
            },
            set: function (value) {
                this._anchorElement['search'] = value;
                this._updateSearchParams();
            },
            enumerable: true
        });

        Object.defineProperties(proto, {

            'toString': {
                get: function () {
                    var _this = this;
                    return function () {
                        return _this.href;
                    };
                }
            },

            'href': {
                get: function () {
                    return this._anchorElement.href.replace(/\?$/, '');
                },
                set: function (value) {
                    this._anchorElement.href = value;
                    this._updateSearchParams();
                },
                enumerable: true
            },

            'pathname': {
                get: function () {
                    return this._anchorElement.pathname.replace(/(^\/?)/, '/');
                },
                set: function (value) {
                    this._anchorElement.pathname = value;
                },
                enumerable: true
            },

            'origin': {
                get: function () {
                    // get expected port from protocol
                    var expectedPort = { 'http:': 80, 'https:': 443, 'ftp:': 21 }[this._anchorElement.protocol];
                    // add port to origin if, expected port is different than actual port
                    // and it is not empty f.e http://foo:8080
                    // 8080 != 80 && 8080 != ''
                    var addPortToOrigin = this._anchorElement.port != expectedPort &&
                        this._anchorElement.port !== '';

                    return this._anchorElement.protocol +
                        '//' +
                        this._anchorElement.hostname +
                        (addPortToOrigin ? (':' + this._anchorElement.port) : '');
                },
                enumerable: true
            },

            'password': { // TODO
                get: function () {
                    return '';
                },
                set: function (value) {
                },
                enumerable: true
            },

            'username': { // TODO
                get: function () {
                    return '';
                },
                set: function (value) {
                },
                enumerable: true
            },
        });

        URL.createObjectURL = function (blob) {
            return _URL.createObjectURL.apply(_URL, arguments);
        };

        URL.revokeObjectURL = function (url) {
            return _URL.revokeObjectURL.apply(_URL, arguments);
        };

        global.URL = URL;

    };

    if (!checkIfURLIsSupported()) {
        polyfillURL();
    }

    if ((global.location !== void 0) && !('origin' in global.location)) {
        var getOrigin = function () {
            return global.location.protocol + '//' + global.location.hostname + (global.location.port ? (':' + global.location.port) : '');
        };

        try {
            Object.defineProperty(global.location, 'origin', {
                get: getOrigin,
                enumerable: true
            });
        } catch (e) {
            setInterval(function () {
                global.location.origin = getOrigin();
            }, 100);
        }
    }

})(
    (typeof global !== 'undefined') ? global
        : ((typeof window !== 'undefined') ? window
            : ((typeof self !== 'undefined') ? self : this))
    );