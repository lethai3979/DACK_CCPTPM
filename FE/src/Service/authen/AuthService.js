// src/services/auth/AuthService.js
import axios from '../api/axiosInstance';
import TokenService from './TokenService';

const AuthService = {
  logIn(credentials) {
    return axios.post('/auth/login', credentials)
      .then(response => {
        TokenService.saveToken(response.data.token);
        return response.data;
      });
  },
  logOut() {
    TokenService.removeToken();
  },
};

export default AuthService;
