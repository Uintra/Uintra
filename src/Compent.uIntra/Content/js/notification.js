export default function() {
    let notification = document.querySelector(".js-notification");
    let notificationList = notification.querySelector(".js-notification-list");

    notification.addEventListener("click", () => notificationList.classList.toggle("hide"));
}