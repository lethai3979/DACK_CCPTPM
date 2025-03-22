// src/services/BookingService.js
import axios from "axios";
import BookingDTO from "../../DTOs/BookingDto";
const BookingService = {
  async AddBooking(bookingDto) {
    const booking = new BookingDTO(bookingDto);
    const jsonData = booking.toJSON();
    const token = sessionStorage.getItem("authToken");
    const response = await axios.post(
      "http://localhost:5027/api/User/Booking/Add",jsonData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    console.log("Response: ",response); 
    if (!response.success) {
      console.log("Booking values invalid", response.success);
    }
    return response;
  },
  async TraCoc(id) {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.post(
      `http://localhost:5027/api/User/Booking/SendCancelRequest/${id}`,id,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Response trả cọc: ",response); 
    if (!response.success) {
      console.log("Booking values invalid", response.success);
    }
    
    return response;
  },
  async ConfirmBooking(id, bien) {
    const formData = new FormData();
    formData.append("id", id);
    formData.append("isAccept", bien);
    axios.defaults.withCredentials = true;
    const token = sessionStorage.getItem("authToken");
    const response = await axios.put(
      `http://localhost:5027/api/User/Booking/ConfirmBooking`,formData,
      {
        headers: {
          Authorization: `Bearer ${token}`
        },
        withCredentials: true
      }
    );
    console.log(response);
    if (response.status !== 200) {
      const errorText = await response.statusText;
      
    }
    return response.data;
  },

  async GetBookingDays(id) {
    console.log("ID: ", id);
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/User/Booking/GetAllBookedDates/${id}`,null,
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
  async GetBookingById(id) {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/User/Booking/GetById/${id}`,
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
  async GetAllBooking() {  // bookign của user 
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/User/Booking/GetPersonalBookings",
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
  async GetAllPenDing() { // Lấy booking đang đợi chủ xe chấp nhận
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/User/Booking/GetAllPendingBookingsByUserId", 
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
export default BookingService;
