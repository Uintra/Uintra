const axios = require('axios');

var ajax = {
    get: url => axios.get(url),
    post: (url, data) => axios.post(url, data),
    put: (url, data) => axios.put(url, data),
    delete: (url, data) => axios.delete(url, data)
}

export default ajax;