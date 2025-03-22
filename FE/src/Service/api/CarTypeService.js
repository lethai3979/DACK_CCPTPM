import CartypeDto from "../../DTOs/CartypeDto";
import axios from "axios";
const CarTypeService = {
  async getCompanies() {
    const response = await axios.get(
      "http://localhost:5027/api/admin/Company/GetAll"
    );
    console.log(response.data);
    return response.data;
  },
  async AddCartype(cartypeDto) {
    const token = sessionStorage.getItem("authToken");
    const jsonData = JSON.stringify(cartypeDto);
    const response = await fetch(
      "http://localhost:5027/api/admin/Cartype/Add",
      {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
        body: jsonData,
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
  async getCartypeById(id) {
    const response = await axios.get(
      `http://localhost:5027/api/admin/Cartype/GetById/${id}`
    );
    console.log(response);
    if (response.status !== 200) {
        throw new Error(`Error: ${response.status} - ${response.statusText}`);
    }

    return response.data;
  },
  async UpdateCarType(cartypeDto){
    const token = sessionStorage.getItem("authToken");
    const jsonData = JSON.stringify(cartypeDto);
    const response = await fetch(`http://localhost:5027/api/admin/Cartype/Update/${cartypeDto.id}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json', // Dữ liệu JSON
        },
        body: jsonData
    });
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(
          `${response.status} - ${response.statusText}: ${errorText}`
        );
      }
  
      return response.json();
  }
};
export default CarTypeService;
