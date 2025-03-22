<template>
    <div>
      <h1>Geolocation Demo</h1>
      <p v-if="error">{{ error }}</p>
      <p v-else-if="position">Your location: {{ position }}</p>
      <button @click="getUserLocation">Get Location</button>
    </div>
  </template>
  
  <script>
import axios from 'axios';

  export default {
    data() {
      return {
        position: null, 
        error: null, 
        response: null    
      };
    },
    methods: {
      getUserLocation() {
        // Kiểm tra xem trình duyệt có hỗ trợ Geolocation API không
        if (!navigator.geolocation) {
          this.error = "Geolocation is not supported by your browser.";
          return;
        }
  
        // Yêu cầu vị trí từ người dùng
        navigator.geolocation.getCurrentPosition(
          (position) => {
            const { latitude, longitude } = position.coords;
            this.position = `Latitude: ${latitude}, Longitude: ${longitude}`;
            const response = axios.get(`https://nominatim.openstreetmap.org/reverse?lat=${latitude}&lon=${longitude}&format=json`);
            console.log(response);
            this.error = null;
          },
          (err) => {
            // Xử lý lỗi nếu người dùng từ chối cấp quyền
            switch (err.code) {
              case err.PERMISSION_DENIED:
                this.error = "Permission denied. Please enable location access.";
                break;
              case err.POSITION_UNAVAILABLE:
                this.error = "Position unavailable.";
                break;
              case err.TIMEOUT:
                this.error = "Request timed out.";
                break;
              default:
                this.error = "An unknown error occurred.";
            }
          }
        );
      },
    },
  };
  </script>
  