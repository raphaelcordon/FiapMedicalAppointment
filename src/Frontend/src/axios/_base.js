import axios from 'axios'


const BASE_URL =
    window.location.hostname === "localhost"
        ? "http://localhost:5002/api"
        : "https://raphaelcordon.com/api";

const AxiosMotion = axios.create({
    baseURL: BASE_URL,
})

export default AxiosMotion