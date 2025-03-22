import UserVM from "./UserVM";
export default class DriverVM {
    constructor(data = {}) {
      this.user = (data.user || []).map(user => new UserVM(user));
      this.ratingPoint = data.ratingPoint || "";
      this.pricePerHour = data.pricePerHour || "";
    }
  }
  