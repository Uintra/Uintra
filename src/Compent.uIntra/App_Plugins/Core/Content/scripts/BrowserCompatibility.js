var btnDisableBrowserCompatibilityNotification;
var browserCompatibilityNotification;

var checkBrowswerCompatibility = {
    init: function () {

        browserCompatibilityNotification = document.querySelector('.js-browser-compatibility-notification');

        if (browserCompatibilityNotification) {

            btnDisableBrowserCompatibilityNotification=document.querySelector('.js-disable-browser-compatibility-notification');

            if (btnDisableBrowserCompatibilityNotification) {
                btnDisableBrowserCompatibilityNotification.addEventListener('click',
                    function() {
                        $.ajax({
                            type: "POST",
                            url: "/umbraco/surface/BrowserCompatibility/DisableBrowserCompatibilityNotification/",
                            success:function() {
                                browserCompatibilityNotification = document.querySelector('.js-browser-compatibility-notification ');
                                if (browserCompatibilityNotification) {
                                    $(browserCompatibilityNotification).hide();
                                }   
                            }
                        });
                    });
            }
        }                
    }    
}
export default checkBrowswerCompatibility;
