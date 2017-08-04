var Quill = require('quill');

const Link = Quill.import('formats/link');
class linkType extends Link {
    static create(value) {
        let node = super.create(value);
        value = this.sanitize(value);
        if (!value.startsWith('https://') && !value.startsWith('http://')) {
            node.setAttribute('href', 'http://' + value);
        }
        return node;
    }
}
Quill.register(linkType);