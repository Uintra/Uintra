import appInitializer from "./AppInitializer";

var dataOldValue = "data-old-value";
var holderSelect = ".js-check-before-unload";
var checkingTypes = "input, select";

function initConfirmOnBeforeUnload() {
    var holders = $(holderSelect);
    for (var i = 0; i < holders.length; i++) {
        saveCurrentValues(holders[i]);
    }

    attachOnBeforeUnloadEvent();

    $(document).on("submit", "form", function() {
        window.onbeforeunload = null;
    });
}

function saveCurrentValues(holder) {
    var elements = $(holder).find(checkingTypes);

    for (var i = 0; i < elements.length; i++) {
        var field = elements[i];

        if (!field.name || field.disabled) {
            continue;
        }

        if (field.type === "select-multiple") {
            for (var j = 0; j < field.options.length; j++) {
                var option = field.options[j];
                if (!option.selected) {
                    continue;
                }

                option.setAttribute(dataOldValue, field.value);
            }
        } else if (field.type !== "checkbox" && field.type !== "radio") {
            field.setAttribute(dataOldValue, field.value);
        } else if (field.checked) {
            field.setAttribute(dataOldValue, field.checked);
        } else if (field.type === "checkbox") {
            field.setAttribute(dataOldValue, field.checked);
        }
    }
}

function attachOnBeforeUnloadEvent() {
    $(window).on('beforeunload', function(){
        var holders = $(holderSelect);

        var result = false;
        for (var i = 0; i < holders.length; i++) {
            result = isHolderValuesChanged(holders[i]);
        }
        if (result) {
            return "Are you sure you want leave?";
        }
    });
}

function isHolderValuesChanged(holder) { 
    var elements = $(holder).find(checkingTypes);

    for (var i = 0; i < elements.length; i++) {
        var field = elements[i];

        if (!field.name || field.disabled) {
            continue;
        }

        if (field.type === "select-multiple") {
            for (var j = 0; j < field.options.length; j++) {
                var option = field.options[j];
                if (!option.selected) {
                    continue;
                }

                if (option.getAttribute(dataOldValue) !== option.value) {
                    return true;
                }
            }
        } else if ((field.type !== "checkbox" && field.type !== "radio")) {
            if (field.getAttribute(dataOldValue) !== field.value) {
                return true;
            }
        }  else if (field.checked) {
            if (toBoolean(field.getAttribute(dataOldValue)) !== field.checked) {
                return true;
            }
        } else if (field.type === "checkbox") {
            if (toBoolean(field.getAttribute(dataOldValue)) !== field.checked) {
                return true;
            }
        }
    }

    return false;
}

function toBoolean(str) {
    switch(str.toLowerCase().trim()){
        case "true": return true;
        case "false": return false;
        default: return false;
    }
}

appInitializer.add(function () {
    initConfirmOnBeforeUnload();
});

export default initConfirmOnBeforeUnload;