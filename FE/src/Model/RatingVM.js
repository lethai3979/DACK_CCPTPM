// src/models/RatingVM.js
import BaseModel from './BaseModelVM';
export default class RatingVM extends BaseModel{
    constructor(data = {}) {
        super(data.id || null, data.createdById, data.createdOn, data.modifiedById, data.modifiedOn, data.isDeleted);
      this.comment = data.comment || '';
      this.point = data.point || 1;
      this.userName = data.userName || '';
      this.userImage = data.userImage || '';
    }
  }
  