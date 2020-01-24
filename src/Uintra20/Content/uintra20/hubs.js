(function ($, window, undefined) {
  /// <param name="$" type="jQuery" />
  "use strict";

  if (typeof ($.signalR) !== "function") {
      throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
  }

  var signalR = $.signalR;

  function makeProxyCallback(hub, callback) {
      return function () {
          // Call the client hub method
          callback.apply(hub, $.makeArray(arguments));
      };
  }

  function registerHubProxies(instance, shouldSubscribe) {
      var key, hub, memberKey, memberValue, subscriptionMethod;

      for (key in instance) {
          if (instance.hasOwnProperty(key)) {
              hub = instance[key];

              if (!(hub.hubName)) {
                  // Not a client hub
                  continue;
              }

              if (shouldSubscribe) {
                  // We want to subscribe to the hub events
                  subscriptionMethod = hub.on;
              } else {
                  // We want to unsubscribe from the hub events
                  subscriptionMethod = hub.off;
              }

              // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
              for (memberKey in hub.client) {
                  if (hub.client.hasOwnProperty(memberKey)) {
                      memberValue = hub.client[memberKey];

                      if (!$.isFunction(memberValue)) {
                          // Not a client hub function
                          continue;
                      }

                      // Use the actual user-provided callback as the "identity" value for the registration.
                      subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue), memberValue);
                  }
              }
          }
      }
  }

  $.hubConnection.prototype.createHubProxies = function () {
      var proxies = {};
      this.starting(function () {
          // Register the hub proxies as subscribed
          // (instance, shouldSubscribe)
          registerHubProxies(proxies, true);

          this._registerSubscribedHubs();
      }).disconnected(function () {
          // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
          // (instance, shouldSubscribe)
          registerHubProxies(proxies, false);
      });

      proxies['notificationsHub'] = this.createHubProxy('notificationsHub');
      proxies['notificationsHub'].client = { };
      proxies['notificationsHub'].server = {
          getNotNotifiedCount: function () {
              return proxies['notificationsHub'].invoke.apply(proxies['notificationsHub'], $.merge(["GetNotNotifiedCount"], $.makeArray(arguments)));
           }
      };

      proxies['previewHub'] = this.createHubProxy('previewHub');
      proxies['previewHub'].client = { };
      proxies['previewHub'].server = {
    };

      proxies['centralFeedHub'] = this.createHubProxy('centralFeedHub');
      proxies['centralFeedHub'].client = { };
      proxies['centralFeedHub'].server = {
      };

      proxies['uSyncHub'] = this.createHubProxy('uSyncHub');
      proxies['uSyncHub'].client = { };
      proxies['uSyncHub'].server = { };

      return proxies;
  };

  signalR.hub = $.hubConnection("/signalr", { useDefaultPath: false });
  $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));
