function readonlyClickEventHandler() {
    alert()
}

function isReadonlyItem(el) {
    return el.dataset.isReadonly.toLowerCase() === "true";
}

var controller = {
    init: function() {
        let selector = ".js-readonly-click-warning";
        var c = 0;
        var items = $(selector);
        console.log(items)
        items.each(function(i, el) { debugger
            if (isReadonlyItem(el)) {
                el.addEventListener('click', readonlyClickEventHandler);
            }
        });

    }
};

export default controller;