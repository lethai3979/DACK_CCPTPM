// src/services/AmenityService.js // Sửa tên file nếu cần
import axios from "axios";
import UserDTO from "../../DTOs/UserDto";
const AuthenticationService = {
  async Login(EmailUser, PassUser) {
    const response = await axios.post(
      "http://localhost:5027/api/Authentication/Login",
      {
        Email: EmailUser,
        Password: PassUser,
      }
    );
    console.log(response);
    if (response.status != 200) {
      console.log("Listring: ", response);
    }

    return response;
  },
  async Logout() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.post(
      "http://localhost:5027/api/Authentication/RemoveUserFromRedis",null,
      {
        headers: {
          Authorization: `Bearer ${token}`
        },
      }
    );
  },
  async SignUp(NameUser, EmailDK, PassDK, PhoneDK) {
    const response = await axios.post(
      "http://localhost:5027/api/Authentication/SignUp",
      {
        UserName: NameUser,
        Email: EmailDK,
        Password: PassDK,
        PhoneNumber: PhoneDK,
      }
    );
    console.log(response);
    if (response.status != 200) {
      const errorText = await response.text();
      throw new Error(
        `${response.status} - ${response.statusText}: ${errorText}`
      );
    }

    return response;
  },

  async getUser() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/Authentication/GetUser`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    if (!response.success) {
      console.log("Lỗi",response);
    }

    return response.data;
  },
  async FindUser(userId) {
    // const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/ManageUser/GetUserInfo/${userId}`, userId,
      // {
      //   headers: {
      //     Authorization: `Bearer ${token}`,
      //   },
      // }
    );
    if(status != 200) {
      console.log("Lỗi : ", response);
    }

    return response.data;
  },
  async UpdateComfirmDriver(userDto) {
    const token = sessionStorage.getItem("authToken");
    const formData = new UserDTO(userDto).toFormData();
    const response = await axios.put(
      `http://localhost:5027/api/User/UpdateUserInfo`,formData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Service: ", response); // Log response từ API
    if (!response.ok) {
      console.log("Lỗi : ", response);
    }
    return response;
  },
};
export default AuthenticationService;
