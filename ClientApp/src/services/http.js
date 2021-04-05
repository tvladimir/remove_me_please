import axios from "axios";

const service = {
    get: axios.get,
    put: axios.put,
    patch: axios.patch,
    post: axios.post,
    delete: axios.delete,
};

export default service;