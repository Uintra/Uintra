require("./faqPanel.css");

export default function initFaqPanel() {
    var panels = document.querySelectorAll(".js-faq-panel");

    [].forEach.call(panels, function(panel) {
        var toggler = panel.querySelector(".js-toggler");
        var body = panel.querySelector(".js-body");
        var holder = panel.querySelector(".js-holder");

        if (!toggler || !body && !holder) return false;

        toggler.addEventListener("click", function () {
            if (panel.classList.contains("_active")) {
                panel.classList.remove("_active");
                body.style.height = "0";
            } else {
                panel.classList.add("_active");
                body.style.height = holder.offsetHeight + "px";
            }

        });
    });
}