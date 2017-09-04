export default function () {
    var currentState;
    let stateSourceElement = document.querySelector("[name='isFiltersOpened']");

    function readState() {
        return $(stateSourceElement).val() === "true";
    }

    function saveState(state) {
        $(stateSourceElement).val(state);
    }

    function applyState(state) {
        let element = $(document.body);
        if (state) {
            element.addClass("_show-filters");
        } else {
            element.removeClass("_show-filters");
        }
    }

    function onFilterTogglerClick() {
        currentState = !currentState;
        saveState(currentState);
        applyState(currentState);
    }

    function initListeners() {
        let filterToggler = document.querySelector(".js-filter__toggle");
        if (!filterToggler) return;
        filterToggler.addEventListener("click", onFilterTogglerClick);
    }

    function init() {
        initListeners();
        currentState = readState();
        applyState(currentState);
    }

    init();
}