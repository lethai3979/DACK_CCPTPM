// src/models/BaseModel.js

export default class BaseModel {
    constructor(id, createdById, createdOn, modifiedById, modifiedOn, isDeleted) {
      this.id = id || null;
      this.createdById = createdById || null;
      this.createdOn = createdOn || new Date(); // Mặc định là ngày hiện tại
      this.modifiedById = modifiedById || null;
      this.modifiedOn = modifiedOn || null;
      this.isDeleted = isDeleted || false; // Mặc định là false
    }
  }
  