export default function() {
    let filterToggler = document.querySelector(".js-filter__toggle");
    if (!filterToggler) return;
    
    filterToggler.addEventListener("click", () => {
        changeInputState();
        changeFiltersState();
    });

    function changeInputState() {
        var isOpened = document.querySelector("[name='isFiltersOpened']");
        var state = $(isOpened).val();
        state === "true" ? $(isOpened).val("false") : $(isOpened).val("true");
    }

    function changeFiltersState() {
        document.body.classList.toggle("_show-filters");
    }
}