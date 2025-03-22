export default class SearchDTO {
    constructor(data = {}) {
      this.company = data.company || "";
      this.seat = data.seat || 0;
      this.gear = data.gear || "";
      this.fuel = data.fuel || "";
      this.hasDriver = data.hasDriver || false;
    }
  }
  