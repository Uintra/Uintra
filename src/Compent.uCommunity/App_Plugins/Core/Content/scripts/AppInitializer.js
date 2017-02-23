'use strict';

var appInitializer = (function () {
    var initializers = [];

    $(document).ready(function () {
        setTimeout(function () {
            initializers.forEach(function (item) { item(); });
        });
    });

    return {
        add: function (cb) {
            initializers.push(cb);
        }
    }
})();

window.App = window.App || {};
window.App.AppInitializer = appInitializer;
