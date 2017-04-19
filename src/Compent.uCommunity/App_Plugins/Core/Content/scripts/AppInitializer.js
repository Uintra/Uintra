var queue = [];

document.addEventListener('DOMContentLoaded', function () {
    setTimeout(function () {
        queue.forEach(function(item) { item(); });
    });
});

var appInitializer = {
    add: function(func) {
        queue.push(func);
    }
}

export default appInitializer;