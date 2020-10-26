(function () {
    'use strict';

    angular
        .module('umbraco')
        .directive('videoLink', videoLinkDirective);

    videoLinkDirective.$inject = [];

    function videoLinkDirective() {
        return {
            scope: {
                videoLink: "=ngModel",
                useAltText: '='
            },
            require: 'ngModel',
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/VideoLink/videoLink.html',
            link: function (scope, element, attrs, ngModelCtrl) {
                var urlregex = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/;
                var youtubeRegExpPatern = /^(?:https?\:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v\=))([\w-]{10,12})(?:[\&\?\#].*?)*?(?:[\&\?\#]t=([\dhm]+s))?$/;
                var vimeoRegExpPattern = /(https?:\/\/)?(www\.)?(player\.)?vimeo\.com\/([a-z]*\/)*([0-9]{6,11})[?]?.*/;
                var hyperlinkRegExpPattern = /((?:https?(?:%3A%2F%2F|:\/\/))(?:www\.)?(?:\S+)(?:%2F|\/)(?:(?!\.(?:mp4|mkv|wmv|m4v|mov|avi|flv|webm|flac|mka|m4a|aac|ogg|3gp|ogv))[^\/])*\.(mp4|mkv|wmv|m4v|mov|avi|flv|webm|flac|mka|m4a|aac|ogg|3gp|ogv))(?!\/|\.[a-z]{1,3})/;
                var youtubeEmbedLink = "https://www.youtube.com/embed/";
                var vimeoEmbededLink = "https://player.vimeo.com/video/";

                var videoSourceTypes = {
                    youtube: 1,
                    vimeo: 2,
                    hyperlink: 3,
                    unknown: 10
                }

                if (!scope.videoLink || (!scope.videoLink.url && !scope.videoLink.altText)) {
                    scope.videoLink = { url: "", altText: "", embedUrl: "", videoId: "", sourceType: "" };
                }

                scope.videoLink.useAltText = scope.useAltText;

                function validateUrl(textval) {
                    return urlregex.test(textval);
                }

                function getYoutubeLink(videoUrl) {
                    var youtubeMatched = videoUrl.match(youtubeRegExpPatern);

                    if (youtubeMatched != null && youtubeMatched[1]) {
                        return {
                            embedLink: youtubeEmbedLink + youtubeMatched[1],
                            videoId: youtubeMatched[1]
                        };
                    }
                    return undefined;
                }

                function getVimeoLink(videoUrl) {
                    var vimeoMatched = videoUrl.match(vimeoRegExpPattern);

                    if (vimeoMatched != null && vimeoMatched[5]) {
                        return {
                            embedLink: vimeoEmbededLink + vimeoMatched[5],
                            videoId: vimeoMatched[5]
                        };
                    }
                    return undefined;
                }

                function getServerLink(videoUrl) {
                    var hyperlinkMatched = videoUrl.match(hyperlinkRegExpPattern);

                    if (hyperlinkMatched != null) {
                        return {
                            embedLink: videoUrl
                        };
                    }
                    return undefined;
                }

                scope.validateLink = function () {
                    var isValid = true;

                    if (scope.videoLink.url) {
                        isValid = validateUrl(scope.videoLink.url);
                    }

                    scope.isRequired = !isValid;
                    ngModelCtrl.$setValidity('videoLink', isValid);

                    if (isValid) {
                        var youtube = getYoutubeLink(scope.videoLink.url);
                        if (youtube) {
                            scope.videoLink.embedUrl = youtube.embedLink;
                            scope.videoLink.videoId = youtube.videoId;
                            scope.videoLink.sourceType = videoSourceTypes.youtube;
                            return;
                        }

                        var vimeo = getVimeoLink(scope.videoLink.url);

                        if (vimeo) {
                            scope.videoLink.embedUrl = vimeo.embedLink;
                            scope.videoLink.videoId = vimeo.videoId;
                            scope.videoLink.sourceType = videoSourceTypes.vimeo;
                            return;
                        }

                        var server = getServerLink(scope.videoLink.url);

                        if (server) {
                            scope.videoLink.embedUrl = server.embedLink;
                            scope.videoLink.sourceType = videoSourceTypes.hyperlink;
                            return;
                        }

                        scope.videoLink.embedUrl = "";
                        scope.videoLink.sourceType = videoSourceTypes.unknown;
                    }
                }

                scope.isError = function () {
                    return ngModelCtrl.$error.videoLink;
                }
            }
        };
    }

})();