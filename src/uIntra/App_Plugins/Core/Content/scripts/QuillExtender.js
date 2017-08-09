var Quill = require('quill');

const Link = Quill.import('formats/link');
class linkType extends Link {

    static sanitize(url) {
        var protocols = ['http', 'https', 'mailto'];

        if (!hasProtocol(url, protocols)) {
            url = 'http://' + url;
        }

        return super.sanitize(url, ['http', 'https', 'mailto']) ? url : this.SANITIZED_URL;
    }


}

function hasProtocol(url, protocols) {
    if (!url) {
        return false;
    }

    return protocols.some(function (element) {
        return url.startsWith(element);
    });
}

Quill.register(linkType);