import ajax from "./Ajax";

function wrapWithSender(s, cb) { return function (d) { cb(s, d) } };

function serialize(form) {
    if (typeof form != "object" || form.nodeName !== "FORM") return "";
    var s = [];

    for (var i = 0; i < form.elements.length; i++) {
        var field = form.elements[i];
        if (!field.name
            || field.disabled
            || field.type === "file"
            || field.type === "reset"
            || field.type === "submit"
            || field.type === "button")
            continue;

        if (field.type === "select-multiple") {
            for (var j = 0; j < field.options.length; j++) {
                var option = field.options[j];
                if (!option.selected) continue;
                s[s.length] = encodeURIComponent(field.name) + "=" + encodeURIComponent(option.value);
            }
        } else if ((field.type !== "checkbox" && field.type !== "radio") || field.checked) {
            s[s.length] = encodeURIComponent(field.name) + "=" + encodeURIComponent(field.value);
        }
    }


    return s.join("&").replace(/%20/g, "+");
};

function onSubmit(e) {
    e.preventDefault();
    var form = e.target;
    submit(form);
}

function submit(form) {
    var action = form.getAttribute("action");
    var data = serialize(form);
    var promise = ajax.Post(action, data);
    promise.then(wrapWithSender(form, onSuccess), wrapWithSender(form, onError));
    return promise;
}

function appendTo(form, data) {
    var appendToSelector = form.dataset['appendTo'];
    if (!appendToSelector) return;
    var elm = form.querySelector(appendToSelector);
    data && (elm.innerHTML = data);
}

function onSuccess(sender, data) {
    appendTo(sender, data);
};

function onError(sender, data) {
    console.error("Some error occured on form submit");
    console.error(sender);
    console.error(data);
};

function umbracoAjaxFormFactory() {
    return function (form) {
        form.addEventListener('submit', onSubmit);
        var submitBtn = form.querySelector('input[type="submit"]');
        if (!submitBtn) {
            submitBtn = document.createElement('input');
            submitBtn.setAttribute('type', 'submit');
            submitBtn.style.display = 'none';
            form.appendChild(submitBtn);
        }

        return {
            form: form,
            reload: function () {
                return submit(form);
            },
            serialize: function() {
                return serialize(form);
            }
        }
    }
}

export default umbracoAjaxFormFactory;