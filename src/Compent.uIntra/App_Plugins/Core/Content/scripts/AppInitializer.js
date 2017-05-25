var queue = [];

document.addEventListener('DOMContentLoaded', function () {
    setTimeout(function () {
        queue.forEach(function(item) {
            if (typeof item === "function") {
                item();
            } else {
                throw new Error("AppInitializer accepts only functions"  + item);
            }
        });
    });
});

var appInitializer = {
    add: function(func) {
        queue.push(func);
    }
}

export default appInitializer;