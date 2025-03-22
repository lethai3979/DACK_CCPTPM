// src/models/CompanyVM.js

import CarTypeDetailVM from './CarTypeDetailVM';
import BaseModel from './BaseModelVM';
export default class CompanyVM extends BaseModel{
  constructor(data = {}) {
    super(data.id || null, data.createdById, data.createdOn, data.modifiedById, data.modifiedOn, data.isDeleted);
    this.name = data.name || '';
    this.iconImage = data.iconImage || '';
    this.carTypeDetail = (data.carTypeDetail || []).map(detail => new CarTypeDetailVM(detail));
  }
}
