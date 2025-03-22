import axios from "axios";
import ReportDTO from "../../DTOs/ReportDto";
const ReportService = {
  async getAllReport() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      "http://localhost:5027/api/ManagerReport/GetAll",
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    return response.data;
  },
  async getReportById(id) {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/ManagerReport/GetById/${id}`
    );
    console.log(response);
    if (response.status !== 200) {
      throw new Error(`Error: ${response.status} - ${response.statusText}`);
    }

    console.log("Report by id: ", response);
    return response.data;
  },
  async AddReport(reportDto) {
    const token = sessionStorage.getItem("authToken");
    const formData = new ReportDTO(reportDto).toFormData();
    const response = await axios.post(
      "http://localhost:5027/api/UserReport/ReportByPostId",
      formData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log(response);
    if (response.status !== 200) {
      console.log("Error: ", response);
    }

    return response.data;
  },

  async SubmitReport(id, bien) {
    console.log(id,bien);
      const formData = new FormData();
      formData.append("id", id);
      formData.append("isAccept", bien);

      // Lấy token từ sessionStorage
      const token = sessionStorage.getItem("authToken");
      const response = await fetch(
        `http://localhost:5027/api/ManagerReport/ExamineReport/${id}`,
        {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${token}`
          },
          body: formData,
        }
      );
      console.log(response);
      return response;
  },

  
};
export default ReportService;
