import BaseModel from './BaseModelVM';
export default class Amenity extends BaseModel{
    constructor(data = {}) {
      super(data.id || null, data.createdById, data.createdOn, data.modifiedById, data.modifiedOn, data.isDeleted);
      this.id = data.id || null;
      this.name = data.name || '';
      this.iconImage = data.iconImage || ''; // Đường dẫn hoặc URL của ảnh icon từ API
    }
  }