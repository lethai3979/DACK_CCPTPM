// src/dtos/CarTypeDTO.js

export default class CarTypeDTO {
  constructor(data = {}) {
    this.id = data.id || 0;
    this.name = data.name || "";
    this.companyIds = data.companyIds || [];
  }

  toFormData() {
    const formData = new FormData();
    if (this.id !== null) {
      // Kiểm tra xem `id` có khác null không
      formData.append("Id", this.id);
    }
    formData.append("Name", this.name);
    this.companyIds.forEach((companyId) =>
      formData.append("CompanyIds", companyId)
    );
    return formData;
  }
}
