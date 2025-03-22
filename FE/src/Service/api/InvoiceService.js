// src/services/InvoiceService.js
import axios from "axios";
const InvoiceService = {
  async getAllInvoicePersonal() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/User/Invoice/GetPersonalInvoices",
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Response Invoice: ",response); 
    if (!response.success) {
      console.log("Invoice values invalid", response.success);
    }
    
    return response.data;
  },
  async CalculateRevenuesByMonth(year) {
    console.log(year);
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/User/Invoice/CalculateRevenuesByMonth/${year}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log(" Chart Invoice: ",response); 
    if (!response.success) {
      console.log("Invoice values invalid", response.success);
    }
    
    return response.data;
  },
};
export default InvoiceService;
