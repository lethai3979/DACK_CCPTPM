// src/models/ReportVM.js

import ReportVM from './ReportVM';
import BaseModel from './BaseModelVM';
export default class ReportTypeVM extends BaseModel {
  constructor(data = {}) {
    super(data.id || null, data.createdById, data.createdOn, data.modifiedById, data.modifiedOn, data.isDeleted);
    this.id = data.id || 0;
    this.name = data.name || '';
    this.reportPoint = data.ReportPoint || 0;
    this.reports = (data.reports || []).map(report => new ReportVM(report));
  }
}
