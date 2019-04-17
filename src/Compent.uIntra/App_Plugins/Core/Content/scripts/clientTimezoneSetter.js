function initClientTimezoneIdSetter() {
    var holder = document.getElementsByClassName("clientTimezoneId");

    if (holder.length) {
        holder[0].value = moment.tz.guess();
    }
}

document.addEventListener("DOMContentLoaded", function () {
	initClientTimezoneIdSetter();
});


