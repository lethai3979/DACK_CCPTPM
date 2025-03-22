// src/services/BookingService.js
import axios from "axios";
import BookingDTO from "../../DTOs/BookingDto";
import SessionStorageService from "../storage/SessionStorageService";
const DriverBookingService = {
  async AddBookingDriver(bookingId) {
    const token = sessionStorage.getItem("authToken");
    console.log("Booking id: ",bookingId);
    const response = await axios.put(
      `http://localhost:5027/api/Driver/AddDriverToBooking/${bookingId}`,bookingId,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Response: ",response); 
    if (!response.success) {
      console.log("Booking values invalid", response.success);
    }
    
    return response;
  },
  async UpdateLocation(latitude, longitude) {
    const token = sessionStorage.getItem("authToken");
    const formData = new FormData();
    formData.append("latitude", latitude);
    formData.append("longitude", longitude);
    const jsonData = JSON.stringify(formData);
    axios.defaults.withCredentials = true;
    const response = await axios.get(
      `http://localhost:5027/api/Driver/UpdateUserLocation/${latitude}&&${longitude}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        // withCredentials: true
      }
    );
    console.log("Response: ",response); 
    if (!response.success) {
      console.log("Booking values invalid", response.success);
    }
    
    return response;
  },
  async HuyChuyen(driverBookingId) {
    console.log("DriverBoookingID: ",driverBookingId);
    const token = sessionStorage.getItem("authToken");
    const response = await axios.post(
      `http://localhost:5027/api/DriverBooking/CancelDriverBooking/${driverBookingId}`,driverBookingId,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Response hủy tài xế: ",response); 
    if (!response.success) {
      console.log("Booking values invalid", response.success);
    }
    
    return response;
  },
  async GetAllDriverBooking() { // tất cả các booking cần tài xế
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/User/Booking/GetAllByDriver",
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Reponse:", response);
    if (!response.success) {
      console.log(response);
    }
    return response.data;
  },
  async GetAllBookingDriverV1(latitude,longitude) { // tất cả các booking cần tài xế và sắp xếp rồi 
    const token = sessionStorage.getItem("authToken");
    console.log(latitude,longitude);
    const response = await axios.get(
      `http://localhost:5027/api/User/Booking/GetAllDriverRequireBookings/${latitude}&&${longitude}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Reponse:", response);
    if (!response.success) {
      console.log(response);
    }
    return response.data;
  },
  async GetAllBookingDriverV2(latitude,longitude) { // tất cả các booking cần tài xế ở gần địa chỉ gửi vào
    const token = sessionStorage.getItem("authToken");
    console.log(latitude,longitude);
    const response = await axios.get(
      `http://localhost:5027/api/User/Booking/GetAllBookingsInRange/${latitude}&&${longitude}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Reponse:", response);
    if (!response.success) {
      console.log(response);
    }
    return response.data;
  },
  async GetAllBookingByDriver() { // danh sách các booking của tài xế
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/User/Booking/GetAllByDriver",
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Reponse:", response);
    if (!response.success) {
      console.log(response);
    }
    return response.data;
  },
  async GetAllBookingByUser() { // danh sách các booking của tài xế
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/User/Booking/GetAllByOwner",
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Reponse:", response);
    if (!response.success) {
      console.log(response);
    }
    return response.data;
  },
  async GetAllDriverBooking() { //danh sách xe đã chạy
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/User/Booking/GetAllByDriver",
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Reponse:", response);
    if (!response.success) {
      console.log(response);
    }
    return response.data;
  },
  async CancelDriverBooking(bookingId) { //danh sách xe đã chạy
    const token = sessionStorage.getItem("authToken");
    const response = await axios.put(
      `http://localhost:5027/api/Driver/RemoveDriverFromBooking/${bookingId}`,bookingId,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Reponse:", response);
    if (!response.success) {
      console.log(response);
    }
    return response.data;
  },
};
export default DriverBookingService;
