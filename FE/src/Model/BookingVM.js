// src/models/BookingVM.js

import PostVM from './PostVM';
import UserVM from './PostVM';
import PromotionVM from './PromotionVM';
import BaseModel from './BaseModelVM';
import DriverVM from './DriverVM';
export default class BookingVM extends BaseModel{
  constructor(data = {}) {
    super(data.id || null, data.createdById, data.createdOn, data.modifiedById, data.modifiedOn, data.isDeleted);
    this.prePayment = data.prePayment || 0;
    this.isRequireDriver = data.isRequireDriver || false;
    this.total = data.total || 0;
    this.finalValue = data.finalValue || 0;
    this.recieveOn = data.recieveOn || null; // Date
    this.returnOn = data.returnOn || null; // Date
    this.status = data.status || '';
    this.isRequest = data.isRequest || false;
    this.isResponse = data.isResponse || false;
    this.longitude = data.longitude || null;
    this.latitude = data.latitude || null;
    this.isPay = data.isPay || false;
    this.user = new UserVM(data.user || {});
    this.post = new PostVM(data.post || {});
    this.promotion = new PromotionVM(data.promotion || {});
    this.driver = new DriverVM(data.driver || {});
  }
}
