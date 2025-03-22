// src/models/PromotionVM.js

import PostPromotionVM from './PostPromotionVM';
import BaseModel from './BaseModelVM';
export default class PromotionVM extends BaseModel{
  constructor(data = {}) {
    super(data.id || null, data.createdById, data.createdOn, data.modifiedById, data.modifiedOn, data.isDeleted);
    this.content = data.content || '';
    this.discountValue = data.discountValue || 0;
    this.expiredDate = data.expiredDate || null; // Date
    this.postPromotions = (data.postPromotions || []).map(promo => new PostPromotionVM(promo));
  }
}
