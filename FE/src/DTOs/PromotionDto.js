// src/dtos/PromotionDTO.js

export default class PromotionDTO {
  constructor(data = {}) {
    this.id = data.id || 0;
      this.content = data.content || '';
      this.discountValue = data.discountValue || 0;
      this.expiredDate = data.expiredDate || null; // Date
      this.postIds = data.postIds || [];
  }

  toFormData() {
      const formData = new FormData();
      if (this.id !== null) {  // Kiểm tra xem `id` có khác null không
        formData.append("Id", this.id);
    }
      formData.append("Content", this.content);
      formData.append("DiscountValue", this.discountValue);
      if (this.expiredDate) {
          formData.append("ExpiredDate", this.expiredDate);
      }
      this.postIds.forEach(postId => formData.append("PostIds", postId));
      return formData;
  }
}
