// src/services/AmenityService.js
import AmenityDTO from "../../DTOs/AmenityDto"; // Sửa tên file nếu cần
const AmenityService = {
  async addAmenity(amenityData) {
    const formData = new AmenityDTO(amenityData).toFormData();
    const token = sessionStorage.getItem("authToken");

    const response = await fetch(
      "http://localhost:5027/api/admin/Amenity/Add",
      {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      }
    );

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `${response.status} - ${response.statusText}: ${errorText}`
      );
    }

    return response.json();
  },

  async getAmenityById(id) {
    const token = sessionStorage.getItem("authToken");
    console.log(id);
    // Sử dụng fetch để đồng bộ với phương thức addAmenity
    const response = await fetch(
      `http://localhost:5027/api/admin/Amenity/GetById/${id}`,
      {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
          Accept: "application/json", // Thêm Accept header nếu cần
        },
      }
    );
    console.log(response);
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `${response.status} - ${response.statusText}: ${errorText}`
      );
    }

    return response.json();
  },
  async UpdateAmenity(amenityData,id) {

    console.log("service:",amenityData);
    console.log(id);
    const formData = new AmenityDTO(amenityData).toFormData();
    const token = sessionStorage.getItem("authToken");

    const response = await fetch(
        `http://localhost:5027/api/admin/Amenity/Update/${id}`,
        {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        }
      );
      console.log(response);
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `${response.status} - ${response.statusText}: ${errorText}`
      );
    }

    return response.json();
  },
};
export default AmenityService;
