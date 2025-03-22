import axios from "axios";

const NotifyService = {
  async getAllNotify(id) {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/Notifiy/GetAllNotify`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log(response);
    if (!response.success) {
      console.log("Lỗi",response);
    }

    return response.data;
  },
  async ReadMesss(id) {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.put(
      `http://localhost:5027/api/Notifiy/MarkAsRead/${id}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log(response);
    if (!response.success) {
      console.log("Lỗi",response);
    }

    return response.data;
  },
  
};
export default NotifyService;
