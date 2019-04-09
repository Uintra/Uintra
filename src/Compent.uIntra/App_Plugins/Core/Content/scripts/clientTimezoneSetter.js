function initClientTimezoneIdSetter() {
    var holder = document.getElementsByClassName("clientTimezoneId");

    if (holder.length) {
        holder[0].value = Intl.DateTimeFormat().resolvedOptions().timeZone;
    }
}

document.addEventListener("DOMContentLoaded", function () {
	initClientTimezoneIdSetter();
});

