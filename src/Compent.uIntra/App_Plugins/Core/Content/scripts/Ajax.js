var formJsonRegex = /^("(((?=\\)\\(["\\\/bfnrt]|u[0-9a-fA-F]{4}))|[^"\\\0-\x1F\x7F]+)*")$/;
var applicationJsonRegex = /^{.+}$/;

function post(url, data, onSuccess, onError, options) {
    if (onSuccess || onError) {
        console.warn("onSuccess and onError callbacks obsolete. use promise instead");
    }

    return new Promise(function (resolve, reject) {
        var request = new XMLHttpRequest();
        request.open('POST', url, true);

        options.headers.forEach(function(header) {
            request.setRequestHeader(header.name, header.value);
        });

        request.onload = () => {
            if (request.status >= 200 && request.status < 400) {
                var responseText = request.responseText;
                var resultData = options.jsonRegex.test(responseText) ? JSON.parse(responseText) : responseText;
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

var ajax = {
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
        let options = {
            headers: [{name: 'Content-Type', value: 'application/x-www-form-urlencoded; charset=UTF-8'}],
            jsonRegex: formJsonRegex
        };
        return post(url, data, onSuccess, onError, options);
    },
    PostJson: function (url, data, onSuccess, onError) {
        let options = {
            headers: [{name: 'Content-Type', value: 'application/json; charset=utf-8'}],
            jsonRegex: applicationJsonRegex
        };
        return post(url, JSON.stringify(data), onSuccess, onError, options);
    }
}

export default ajax;