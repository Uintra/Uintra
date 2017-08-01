const uIntra = window.uIntra = {
    methods: {
        add: function (name, method) {
            this[name] = method;
            this.list.push(name);
        },

        exist: function (name) {
            return this.list.includes(name);
        },

        list: []
    },
    variables: {
        add: function (name, value) {
            this[name] = value;
            this.list.push(value);
        },

        exist: function (name) {
            return this.list.includes(name);
        },

        list: []
    },
    events: {
        add: function (name, params) {
            this[name] = {
                eventName: name,
                eventBody: new CustomEvent(name, params),
                dispatch: function() {
                    document.body.dispatchEvent(this.eventBody);
                }
            };

            this.list.push(name);

            if (this._timeoutListeners[name]) {
                document.body.addEventListener(name, this._timeoutListeners[name].bind(this));
                delete this._timeoutListeners[name];
            }
        },
        addListener: function(eventName, callback) {
            if (this.exist(eventName)) {
                document.body.addEventListener(eventName, callback);
            } else {
                this._timeoutListeners[eventName] = callback;
            }
        },
        exist: function (name) {
            return this.list.includes(name);
        },

        list: [],

        _timeoutListeners: {}
    }
};

export {uIntra};