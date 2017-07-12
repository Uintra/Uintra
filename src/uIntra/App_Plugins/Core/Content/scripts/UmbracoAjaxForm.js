import ajax from "./Ajax";
import helpers from "./Helpers";

function wrapWithSender(s, cb) { return function (d) { cb(s, d) } };

function onSubmit(e) {
    e.preventDefault();
    var form = e.target;
    submit(form);
}

function submit(form) {
    var action = form.getAttribute("action");
    var data = helpers.serialize(form);
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


function umbracoAjaxFormFactory(form) {
    if (!form) {
        return;
    }

    form.addEventListener('submit', onSubmit);
    var submitBtn = form.querySelector('input[type="submit"]');
    if (!submitBtn) {
        submitBtn = document.createElement('input');
        submitBtn.setAttribute('type', 'submit');
        submitBtn.style.display = 'none';
        $(form).append(submitBtn);

    }

    return {
        form: form,
        reload: function () {
            return submit(form);
        },
        submit: function() {
            return submit(form);
        }
    }
}

export default umbracoAjaxFormFactory;