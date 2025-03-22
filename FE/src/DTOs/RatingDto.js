// src/dtos/RatingDTO.js

export default class RatingDTO {
  constructor(data = {}) {
    this.id = data.id || 0;
      this.comment = data.comment || '';
      this.point = data.point || 1;
      this.postId = data.postId || 0;
  }

  toFormData() {
      const formData = new FormData();
      if (this.id !== 0) {  // Kiểm tra xem `id` có khác null không
        formData.append("Id", this.id);
    }
      formData.append("Comment", this.comment);
      formData.append("Point", this.point);
      formData.append("PostId", this.postId);
      return formData;
  }
}
