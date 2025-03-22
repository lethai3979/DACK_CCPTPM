// src/services/auth/TokenService.js

const TokenService = {
    saveToken(token) {
      localStorage.setItem('auth_token', token);
    },
    getToken() {
      return localStorage.getItem('auth_token');
    },
    getUser() {
      return localStorage.getItem('User');
    },
    removeToken() {
      sessionStorage.removeItem('User');
      sessionStorage.removeItem('authToken');
    }
  };
  
  export default TokenService;
  