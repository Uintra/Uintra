var controller = {
    init: function () {
        var selector = ".js-group-subscribe ._unsubscribe";
        var unsubscribeBtn = $(selector);
        unsubscribeBtn.click(el => alert(el))
    }
}

export default controller;
