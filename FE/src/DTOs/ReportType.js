// src/dtos/ReportTypeDTO.js

export default class ReportTypeDTO {
    constructor(data = {}) {
      this.id = data.id || 0;
      this.name = data.name || "";
      this.reportPoint = data.reportPoint || 0;
    }
  
    toFormData() {
      const formData = new FormData();
      if (this.id !== null) {
        formData.append("Id", this.id);
      }
      formData.append("Name", this.name);
      formData.append("ReportPoint", this.reportPoint);
      return formData;
    }
  }
  