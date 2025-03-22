// src/services/api/axiosInstance.js
import axios from 'axios';

const instance = axios.create({
  baseURL: "http://localhost:5027",
  
});

instance.urlImage = "http://localhost:5027/images/";

instance.interceptors.response.use(
  response => response,
  error => {
    return Promise.reject(error);
  }
);

export default instance;
