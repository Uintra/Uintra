var ajax = (function () {
    var jsonRegex = /^("(((?=\\)\\(["\\\/bfnrt]|u[0-9a-fA-F]{4}))|[^"\\\0-\x1F\x7F]+)*")$/;
    return {
        Get: function (url, onSuccess, onError) {
            if (onSuccess || onError) {
                console.warn("onSuccess and onError callbacks obsolete. use promise instead");
            }

            return new Promise(function (resolve, reject) {
                var request = new XMLHttpRequest();
                request.open('GET', url, true);
                request.onload = () => {
                    if (request.status >= 200 && request.status < 400) {
                        var responseText = request.responseText;
                        var resultData = jsonRegex.test(responseText) ? JSON.parse(responseText) : responseText;
                        onSuccess && onSuccess(resultData);
                        resolve(resultData);
                    } else {
                        onError && onError({ code: request.status, message: request.responseText });
                        reject({ code: request.status, message: request.responseText });
                    }
                }
                request.onerror = onError;
                request.send();
            });
        },
        Post: function (url, data, onSuccess, onError) {
            if (onSuccess || onError) {
                console.warn("onSuccess and onError callbacks obsolete. use promise instead");
            }

            return new Promise(function (resolve, reject) {
                var request = new XMLHttpRequest();
                request.open('POST', url, true);
                request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
                request.onload = () => {
                    if (request.status >= 200 && request.status < 400) {
                        var responseText = request.responseText;
                        var resultData = jsonRegex.test(responseText) ? JSON.parse(responseText) : responseText;
                        onSuccess && onSuccess(resultData);
                        resolve(resultData);
                    } else {
                        onError && onError({ code: request.status, message: request.responseText });
                        reject({ code: request.status, message: request.responseText });
                    }
                }
                request.onerror = onError;
                request.send(data);
            });
        }
    }
})();

window.App = window.App || {};
window.App.Ajax = ajax;