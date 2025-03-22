// src/models/PostVM.js

import PostImageVM from './PostImageVM';
import UserVM from './UserVM';
import PostAmenityVM from './PostAmenityVM';
import PostPromotionVM from './PostPromotionVM';
import RatingVM from './RatingVM';
import BaseModel from './BaseModelVM';
export default class PostVM extends BaseModel{
  constructor(data = {}) {
    super(data.id || null, data.createdById, data.createdOn, data.modifiedById, data.modifiedOn, data.isDeleted);
    this.name = data.name || '';
    this.image = data.image || '';
    this.images = (data.images || []).map(img => new PostImageVM(img));
    this.description = data.description || '';
    this.seat = data.seat || 0;
    this.rentLocation = data.rentLocation || '';
    this.hasDriver = data.hasDriver || false;
    this.pricePerHour = data.pricePerHour || 0;
    this.pricePerDay = data.pricePerDay || 0;
    this.gear = data.gear || false;
    this.fuel = data.fuel || '';
    this.fuelConsumed = data.fuelConsumed || 0;
    this.rideNumber = data.rideNumber || 0;
    this.avgRating = data.avgRating || 0;
    this.isAvailable = data.isAvailable || false;
    this.isDisabled = data.isDisabled || false;
    this.carTypeId = data.carTypeId || 0;
    this.carTypeName = data.carTypeName || '';
    this.companyId = data.companyId || 0;
    this.companyName = data.companyName || '';
    this.user = new UserVM(data.user || {});
    this.postAmenities = (data.postAmenities || []).map(amenity => new PostAmenityVM(amenity));
    this.postPromotions = (data.postPromotions   || []).map(promo => new PostPromotionVM(promo));
    this.ratings = (data.ratings || []).map(rating => new RatingVM(rating));
  }
}
