import UserVM from "./UserVM";
export default class NotifyVM {
    constructor(data = {}) {
      this.id = data.id || 0;
      this.bookingId = data.bookingId || 0;
      this.title = data.title|| 0;
      this.content = data.content || '';
      this.createOn = data.createOn || 0;
      this.isRead = data.isRead || '';
      this.user = (data.user || []).map(user => new UserVM(user));
    }
  }
  

