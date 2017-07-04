const uIntra = window.uIntra = {
    methods: {
        add: function (name, method) {
            this[name] = method;
            this.list.push({
                [name]: method
            })
        },

        list: []
    },
    variables: {
        add: function (name, value) {
            this[name] = value;
            this.list.push({
                [name]: value
            })
        },

        list: []
    }
};

export {uIntra};