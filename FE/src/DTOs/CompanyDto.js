// src/dtos/CompanyDTO.js

export default class CompanyDTO {
  constructor(data = {}) {
    this.id = data.id || 0;
    this.name = data.name || "";
    this.iconImage = data.iconImage || null; // File hình ảnh
    this.carTypeIds = data.carTypeIds || [];
  }

  toFormData() {
    const formData = new FormData();
    if (this.id !== null) {
      // Kiểm tra xem `id` có khác null không
      formData.append("Id", this.id);
    }
    formData.append("Name", this.name);
    if (this.iconImage) {
      formData.append("IconImage", this.iconImage); // File hình ảnh
    }
    this.carTypeIds.forEach((carTypeId) =>
      formData.append("CarTypeIds", carTypeId)
    );
    return formData;
  }
}
