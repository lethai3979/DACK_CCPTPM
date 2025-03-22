// src/services/BookingService.js
import axios from "axios";
import BookingDTO from "../../DTOs/BookingDto";
const BookingRequestService = {
  async getAllCancel() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/BookingRequest/GetAllCancelRequest",
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
    
    return response.data;
  },
  async getAllRefundInvoice() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/BookingRequest/GetAllRefundInvoice",
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

  async ConfirmBookingRquest(bookingId, isAccept) {
    console.log(bookingId,isAccept);
    const formData = new FormData();
    formData.append("bookingId", bookingId);
    formData.append("isAccept", isAccept);

    const token = sessionStorage.getItem("authToken");
    const response = await axios.post(
      `http://localhost:5027/api/BookingRequest/ExamineCancelBooking/${bookingId}&&${isAccept}`,formData,
      {
        headers: {
          Authorization: `Bearer ${token}`
        },
      }
    );
    console.log(response);
    return response;
  },
};
export default BookingRequestService;
