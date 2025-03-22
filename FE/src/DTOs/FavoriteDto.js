// src/dtos/AmenityDTO.js

export default class FavoriteDTO {
    constructor(data = {}) {
      this.id = data.id || null;
      this.postId = data.postId || null;
    }
    toFormData() {
      const formData = new FormData();
      if (this.id !== null) {
        formData.append("Id", this.id);
      }
      formData.append("PostId", this.postId); 
      return formData;
    }
    toJSON() {
      return {
        // id: this.id,
        postId: this.postId,
      };
    }
  }
  