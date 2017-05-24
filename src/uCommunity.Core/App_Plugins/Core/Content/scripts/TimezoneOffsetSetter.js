import appInitializer from "./AppInitializer";

function initTimezoneOffsetSetter() {
    var holder = document.getElementsByClassName("clientTimezoneOffset");

    if (holder.length) {
        var currentDate = new Date();
        holder[0].value = currentDate.getTimezoneOffset();
    }
}

appInitializer.add(function() {
    initTimezoneOffsetSetter();
});

export default initTimezoneOffsetSetter;