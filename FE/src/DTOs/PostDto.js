// src/dtos/PostDTO.js

export default class PostDTO {
  constructor(data = {}) {
    this.id = data.id || 0;
    this.name = data.name || "";
    this.image = data.image || null; // File hình ảnh chính
    this.imagesList = data.imagesList || []; // Danh sách các file hình ảnh phụ
    this.description = data.description || "";
    this.seat = data.seat || 0;
    this.rentLocation = data.rentLocation || "";
    this.hasDriver = data.hasDriver || false;
    this.pricePerHour = data.pricePerHour || 0;
    this.pricePerDay = data.pricePerDay || 0;
    this.gear = data.gear || false;
    this.fuel = data.fuel || "";
    this.fuelConsumed = data.fuelConsumed || 0;
    this.carTypeId = data.carTypeId || 0;
    this.companyId = data.companyId || 0;
    this.amenitiesIds = data.amenitiesIds || [];
  }

  toFormData() {
    const formData = new FormData();
    if (this.id !== null) {
      // Kiểm tra xem `id` có khác null không
      formData.append("Id", this.id);
    }
    formData.append("Name", this.name);
    if (this.image) {
      formData.append("Image", this.image); // File hình ảnh chính
    }
    this.imagesList.forEach((img) => formData.append("ImagesList", img)); // Danh sách hình ảnh phụ
    formData.append("Description", this.description);
    formData.append("Seat", this.seat);
    formData.append("RentLocation", this.rentLocation);
    formData.append("HasDriver", this.hasDriver);
    formData.append("PricePerHour", this.pricePerHour);
    formData.append("PricePerDay", this.pricePerDay);
    formData.append("Gear", this.gear);
    formData.append("Fuel", this.fuel);
    formData.append("FuelConsumed", this.fuelConsumed);
    formData.append("CarTypeId", this.carTypeId);
    formData.append("CompanyId", this.companyId);
    this.amenitiesIds.forEach((amenityId) =>
      formData.append("AmenitiesIds", amenityId)
    );
    return formData;
  }
}
