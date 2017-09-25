import confirm from './../../Controls/Confirm/Confirm';

function readonlyClickEventHandler(e) {
    e.preventDefault();
    var element = $(this);
    var title = element.find("[name='readonlyClickTitle']").val()
    var message = element.find("[name='readonlyClickMessage']").val();
    confirm.alert(title, message);
}

function isReadonlyItem(el) {
    return el.dataset.isReadonly.toLowerCase() === "true";
}

var controller = {
    init: function() {
        let selector = ".js-readonly-click-warning";
        let items = $(selector);      
        items.each(function(i, el) {
            if (isReadonlyItem(el)) {
                el.addEventListener('click', readonlyClickEventHandler);
            }
        });

    }
};

export default controller;