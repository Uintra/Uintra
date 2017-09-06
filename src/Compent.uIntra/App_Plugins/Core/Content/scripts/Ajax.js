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
                var resultData = jsonParseSafe(responseText);
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

function jsonParseSafe(text) {
    try {
        return JSON.parse(text);
    }
    catch(err){
        
    }
    return text;
}

var ajax = {
    Get: function (url, onSuccess, onError) {
        if (onSuccess || onError) {
            console.warn("onSuccess and onError callbacks obsolete. use promise instead");
        }

        return new Promise(function (resolve, reject) {
            var request = new XMLHttpRequest();
            request.open('GET', url, true);
            request.setRequestHeader('Cache-Control', 'max-age=0');
            request.setRequestHeader('Cache-control', 'no-cache');
            request.setRequestHeader('Cache-control', 'no-store');
            request.setRequestHeader('Pragma', 'no-cache');
            request.setRequestHeader('Expires', '0');
            request.onload = () => {
                if (request.status >= 200 && request.status < 400) {
                    var responseText = request.responseText;
                    var resultData = formJsonRegex.test(responseText) ? JSON.parse(responseText) : responseText;
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
            headers: [
                {name: 'Content-Type', value: 'application/x-www-form-urlencoded; charset=UTF-8'},
                {name: 'Cache-Control', value: 'max-age=0'},
                {name: 'Cache-Control', value: 'no-cache'},
                {name: 'Cache-Control', value: 'no-store'},
                {name: 'Pragma', value: 'no-cache'},
                {name: 'Expires', value: '0'}
            ],
            jsonRegex: formJsonRegex
        };
        return post(url, data, onSuccess, onError, options);
    },
    PostJson: function (url, data, onSuccess, onError) {
        let options = {
            headers: [
                {name: 'Content-Type', value: 'application/json; charset=utf-8'},
                {name: 'Cache-Control', value: 'max-age=0'},
                {name: 'Cache-Control', value: 'no-cache'},
                {name: 'Cache-Control', value: 'no-store'},
                {name: 'Pragma', value: 'no-cache'},
                {name: 'Expires', value: '0'}
            ],
            jsonRegex: applicationJsonRegex
        };
        return post(url, JSON.stringify(data), onSuccess, onError, options);
    },
    Delete: function (url, onSuccess, onError) {
        if (onSuccess || onError) {
            console.warn("onSuccess and onError callbacks obsolete. use promise instead");
        }

        return new Promise(function (resolve, reject) {
            var request = new XMLHttpRequest();
            request.open('DELETE', url, true);
            request.setRequestHeader('Cache-Control', 'max-age=0');
            request.setRequestHeader('Cache-control', 'no-cache');
            request.setRequestHeader('Cache-control', 'no-store');
            request.setRequestHeader('Pragma', 'no-cache');
            request.setRequestHeader('Expires', '0');

            request.onload = () => {
                if (request.status >= 200 && request.status < 400) {
                    var responseText = request.responseText;
                    var resultData = formJsonRegex.test(responseText) ? JSON.parse(responseText) : responseText;
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
    }
}

export default ajax;