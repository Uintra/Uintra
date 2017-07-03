import initSearchBox from './Controls/search-box';
import initSearchResult from './Result/search-result';

require('./styles.css');

export default function () {
    initSearchBox();
    initSearchResult();
}