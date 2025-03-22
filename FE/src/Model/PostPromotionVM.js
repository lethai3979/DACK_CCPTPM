// src/models/PostPromotionVM.js

export default class PostPromotionVM {
    constructor(data = {}) {
      this.id = data.id || 0;
      this.promotionId = data.promotionId || 0;
      this.promotionContent = data.promotionContent || '';
      this.postId = data.postId || 0;
      this.postName = data.postName || '';
    }
  }
  