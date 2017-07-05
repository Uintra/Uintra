const uIntra = window.uIntra = {
    methods: {
        add: function (name, method) {
            this[name] = method;
            this.list.push(name);
        },

        exist: function (name) {
            return this.list.indexOf(name) !== -1
        },

        list: []
    },
    variables: {
        add: function (name, value) {
            this[name] = value;
            this.list.push(value);
        },

        exist: function (name) {
            return this.list.indexOf(name) !== -1
        },

        list: []
    }
};

export {uIntra};