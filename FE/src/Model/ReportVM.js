// src/models/ReportVM.js

import PostVM from './PostVM';

export default class ReportVM {
  constructor(data = {}) {
    this.id = data.id || 0;
    this.content = data.content || '';
    this.postId = data.postId || 0;
    this.post = new PostVM(data.post || {});
    this.reportId = data.reportId || 0;
    this.reportName = data.reportName || '';
  }
}
