require('./style.css');

import bulletinsCreate from './Create/bulletins.create';
import bulletinsEdit from './Edit/bulletins.edit';

export default function () {
    bulletinsCreate.init();
    bulletinsEdit.init();
}