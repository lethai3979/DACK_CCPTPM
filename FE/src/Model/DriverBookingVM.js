import DriverVM from "./DriverVM";
import InvoiceVM from "./InvoiceVM"
export default class DriverBookingVM {
    constructor(data = {}) {
      
      this.pickUpDate = data.pickUpDate || "";
      this.dropOffDate = data.dropOffDate || "";
      this.total = data.total || 0;
      this.isCancel = data.isCancel || "";
      this.driverId = data.driverId || "";
      this.driver = (data.driver || []).map(driver => new DriverVM(driver));
      this.invoice = (data.invoice || []).map(invoice => new InvoiceVM(invoice));
    }
  }
  

