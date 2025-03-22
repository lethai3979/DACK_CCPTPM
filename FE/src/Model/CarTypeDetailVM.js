// src/models/CarTypeDetailVM.js

export default class CarTypeDetailVM {
    constructor(data = {}) {
      this.id = data.id || 0;
      this.carTypeId = data.carTypeId || 0;
      this.carTypeName = data.carTypeName || '';
      this.companyId = data.companyId || 0;
      this.companyName = data.companyName || '';
      this.isDeleted = data.isDeleted || false;
    }
  }
  