// src/dtos/ReportDTO.js

export default class ReportDTO {
  constructor(data = {}) {
    this.id = data.id || 0;
    this.content = data.content || "";
    this.postId = data.postId || 0;
    this.reportTypeId = data.reportTypeId || 0;
  }

  toFormData() {
    const formData = new FormData();
    if (this.id !== null) {
      // Kiểm tra xem `id` có khác null không
      formData.append("Id", this.id);
    }
    formData.append("Content", this.content);
    formData.append("PostId", this.postId);
    formData.append("ReportTypeId", this.reportTypeId);
    return formData;
  }
}
