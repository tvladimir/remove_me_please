import http from "./http";
import { apiUrl } from "../config.json";


export function createTask(file, text) {
    const url = `${apiUrl}/saveNew`;
    const formData = new FormData();
    formData.append('file', file);
    formData.append('text', text);
    const config = {
        headers: {
            'content-type': 'multipart/form-data',
        }
    }
    return http.post(url, formData, config);
}

export function getAllTasks() {
    const url = `${apiUrl}/data`;
    try {
        return http.get(url);
    } catch (err) {
        console.error("Error response:");
        console.error(err.response.data);
        console.error(err.response.status);
        console.error(err.response.headers);
    }
}

const service = {
    createTask,
    getAllTasks,
};

export default service;