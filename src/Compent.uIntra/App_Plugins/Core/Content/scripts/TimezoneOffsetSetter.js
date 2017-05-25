function initTimezoneOffsetSetter() {
    var holder = document.getElementsByClassName("clientTimezoneOffset");

    if (holder.length) {
        var currentDate = new Date();
        holder[0].value = currentDate.getTimezoneOffset();
    }
}

document.addEventListener("DOMContentLoaded", function () {
    initTimezoneOffsetSetter();
});

