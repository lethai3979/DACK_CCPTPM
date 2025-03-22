import CompanyDto from "../../DTOs/CompanyDto";
import axios from "axios";
const CompanyService = {
  async getCarTypies() {
    const response = await axios.get(
      "http://localhost:5027/api/admin/Cartype/GetAll"
    );
    console.log(response.data);
    return response.data;
  },
  async AddCompany(companyDto) {
    const formData = new CompanyDto(companyDto).toFormData();
    const token = sessionStorage.getItem("authToken");
    const response = await fetch(
      "http://localhost:5027/api/admin/Company/Add",
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
  async getCompanyById(id) {
    const response = await axios.get(
      `http://localhost:5027/api/admin/Company/GetById/${id}`
    );
    console.log(response);
    if (response.status !== 200) {
        throw new Error(`Error: ${response.status} - ${response.statusText}`);
    }
    return response.data;
  },
  async UpdateCompany(companyDto){
    
    const formData = new CompanyDto(companyDto).toFormData();
    console.log(formData);
    const token = sessionStorage.getItem("authToken");
    const response = await fetch(`http://localhost:5027/api/admin/Company/Update/${companyDto.id}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
        body: formData
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
export default CompanyService;
