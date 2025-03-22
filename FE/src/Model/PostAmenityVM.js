// src/models/PostAmenityVM.js

export default class PostAmenityVM {
    constructor(data = {}) {
      this.id = data.id || 0;
      this.postId = data.postId || 0;
      this.postName = data.postName || '';
      this.amenityId = data.amenityId || 0;
      this.amenityName = data.amenityName || '';
      this.amenityIconImage = data.amenityIconImage || '';
      this.isDeleted = data.isDeleted || false;
    }
  }
  