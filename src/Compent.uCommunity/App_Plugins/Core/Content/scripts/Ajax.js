var ajax = (function () {
    var noop = function () { };
    var sendRquest = function (request, data, success, error) {
        request.addEventListener("load",
            () => {
                if (request.status >= 200 && request.status < 400) {
                    (success || noop)(JSON.parse(request.response));
                } else {
                    (error || noop)(request.response);
                }
            });

        request.addEventListener("error",
            () => {
                (error || (() => { }))(request.response);
                if (request.status == 403) {

                }
            });

        request.send(JSON.stringify(data));
    }

    return {
        Get: function (url, success, error) {
            var request = new XMLHttpRequest();
            request.open('GET', url);
            sendRquest(request, undefined, success, error);
        },
        Post: function (url, data, success, error) {
            var request = new XMLHttpRequest();
            request.open('POST', url);
            request.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            sendRquest(request, data, success, error);
        }
    }
})();

window.App = window.App || {};
window.App.Ajax = ajax;