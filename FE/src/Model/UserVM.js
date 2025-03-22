import DriverVM from "./DriverVM";
export default class UserVM {
  constructor(data = {}) {
    this.id = data.id || null;
    this.name = data.name || "";
    this.createDate = data.createDate || null;
    this.email = data.email || "";
    this.phoneNumber = data.phoneNumber || "";
    this.lockoutEnd = data.lockoutEnd || null;
    this.lockoutEnabled = data.lockoutEnabled || false;
    this.isSubmitDriver = data.isSubmitDriver || false;
    this.cic = data.cic || "";
    (this.license = data.license || ""),
      (this.reportPoint = data.reportPoint || 0),
      (this.image = data.image || ""); // Đường dẫn hoặc URL của ảnh icon từ API
    (this.birthday = data.birthday || null), (this.roles = data.roles || []);
  }
}


