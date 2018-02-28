import axios from 'axios';

let config = { 
    transformResponse: [function (data) {
        try {
            return JSON.parse(data);
        } catch (e) {
            return data;
        }
    }],
    headers: { 'Cache-Control': 'no-cache', 'Pragma': 'no-cache' }
}

let ajax = {    
    get: url => axios.get(url, config),
    post: (url, data) => axios.post(url, data, config),
    put: (url, data) => axios.put(url, data, config),
    delete: (url, data) => axios.delete(url, data, config)
}

export default ajax;