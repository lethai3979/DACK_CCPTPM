import ReportTypeDTO from "../../DTOs/ReportType";
import axios from "axios";
const ReportTypeService = {
    async AddReportType(reportType) {
        const token = sessionStorage.getItem('authToken');
        const formData = new ReportTypeDTO(reportType).toFormData();
        const response = await fetch(
            "http://localhost:5027/api/admin/ReportType/Add",
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
    async GetAll() {
        const response = await axios.get("http://localhost:5027/api/Admin/ReportType/GetAll");
        // console.log(response);
          if (response.status != 200) {
            const errorText = await response.text();
            throw new Error(
              `${response.status} - ${response.statusText}: ${errorText}`
            );
          }
          return response.data;
    },
    async GetById(id) {
        const response = await axios.get(`http://localhost:5027/api/admin/ReportType/GetById/${id}`);
          if (response.status != 200) {
            const errorText = await response.text();
            throw new Error(
              `${response.status} - ${response.statusText}: ${errorText}`
            );
          }
          return response.data;
    },

}
export default ReportTypeService;