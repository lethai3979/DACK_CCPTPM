// src/services/SessionStorageService.js

const SessionStorageService = {
  setItem(key, value) {
    sessionStorage.setItem(key, JSON.stringify(value));
  },
  getItem(key) {
    const item = sessionStorage.getItem(key);
    return item ? JSON.parse(item) : null;
  },
  removeItem(key) {
    sessionStorage.removeItem(key);
  },
  clear() {
    sessionStorage.clear();
  },
  getCookie() {
    const allCookies = document.cookie;

    // Lấy giá trị của cookie .AspNetCore.Session
    const sessionCookie = allCookies
      .split("; ")
      .find((row) => row.startsWith(".AspNetCore.Session="))
      ?.split("=")[1]; // Giá trị của cookie

    return sessionCookie;
  },
};

export default SessionStorageService;
