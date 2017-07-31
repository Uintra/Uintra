var checkContainer;
var checkBrowswerCompatibility = {
    initCheck: function (holder) {
        var checkContainer = holder.find('.js-browser-compatibility');

        if (checkContainer) {
            $.ajax({
                type: "POST",
                url: "/umbraco/surface/BrowserCompatibility/CheckBrowserCompatibility/"
            });
        }
    }
}