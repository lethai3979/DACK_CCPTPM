// src/dtos/AmenityDTO.js

export default class AmenityDTO {
  constructor(data = {}) {
    this.id = data.id || null;
    this.name = data.name || "";
    this.iconImage = data.iconImage || null;
  }
  toFormData() {
    const formData = new FormData();
    if (this.id !== null) {
      // Kiểm tra xem `id` có khác null không
      formData.append("Id", this.id);
    }
    formData.append("Name", this.name); // Thêm tên tiện nghi
    if (this.iconImage) {
      formData.append("IconImage", this.iconImage); // Thêm file hình ảnh nếu có
    }
    return formData;
  }
}
