export default function() {
    let notification = document.querySelector(".js-notification");
    let notificationList = notification.querySelector(".js-notification-list");

    notification.addEventListener("click", (e) => {
        e.stopPropagation();
        if (notificationList.classList.contains("hide")) {
            notificationList.classList.remove("hide");
            document.body.addEventListener("click", hidenotificationList);
        } else {
            notificationList.classList.add("hide");
            document.body.removeEventListener("click", hidenotificationList);
        }
    });

    function hidenotificationList() {
        notificationList.classList.add("hide");
    }
}