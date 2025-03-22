// src/dtos/BookingDTO.js

export default class BookingDTO {
  constructor(data = {}) {
    this.id = data.id || 0;
    this.prePayment = data.prePayment || 0;
    this.total = data.total || 0;
    this.finalValue = data.finalValue || 0;
    this.longitude = data.longitude || null; // Date
    this.latitude = data.latitude || null; // Date
    this.recieveOn = data.recieveOn || null; // Date
    this.returnOn = data.returnOn || null; // Date
    this.postId = data.postId || 0;
    this.promotionId = data.promotionId || null;
    this.discountValue = data.discountValue || 0;
    this.isRequireDriver = data.isRequireDriver || false;
  }

  toFormData() {
    const formData = new FormData();
    if (this.id !== null) {
      // Kiểm tra xem `id` có khác null không
      formData.append("Id", this.id);
    }
    formData.append("PrePayment", this.prePayment);
    formData.append("Total", this.total);
    formData.append("FinalValue", this.finalValue);
    if (this.recieveOn) {
      formData.append("RecieveOn", this.recieveOn);
    }
    if (this.returnOn) {
      formData.append("ReturnOn", this.returnOn);
    }
    formData.append("Longitude", this.longitude);
    formData.append("Latitude", this.latitude);
    formData.append("PostId", this.postId);
    // formData.append("PromotionId", this.promotionId);
    if (this.promotionId !== null) {
      // Kiểm tra xem `id` có khác null không
      formData.append("PromotionIdd", this.promotionId);
    }
    formData.append("DiscountValue", this.discountValue);
    formData.append("IsRequireDriver", this.isRequireDriver);

    return formData;
  }
  toJSON() {
    return {
      id: this.id,
      prePayment: this.prePayment,
      total: this.total,
      finalValue: this.finalValue,
      recieveOn: this.recieveOn,
      returnOn: this.returnOn,
      longitude: this.longitude.toString(),
      latitude: this.latitude.toString(),
      isRequireDriver: this.isRequireDriver,
      postId: this.postId,
      promotionId: this.promotionId,
      discountValue: this.discountValue,
    };
  }
}
