const uIntra = window.uIntra = {
    methods: {
        add: function (name, method) {
            this[name] = method;
            this.list.push(name);
        },

        exist: function (name) {
            return this.list.indexOf(name) !== -1;
        },

        list: []
    },
    variables: {
        add: function (name, value) {
            this[name] = value;
            this.list.push(value);
        },

        exist: function (name) {
            return this.list.indexOf(name) !== -1;
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
            }

            this.list.push(name);
        },
        addListener: function(eventName, callback) {
            if(this.exist(eventName)){
                document.body.addEventListener(eventName, callback);
            }
        },
        exist: function (name) {
            return this.list.indexOf(name) !== -1;
        },

        list: []
    }
};

export {uIntra};