// src/dtos/UserDTO.js

export default class UserDTO {
  constructor(data = {}) {
    this.name = data.name || "";
    this.phonenumber = data.phonenumber || "";
    this.cic = data.cic || "";
    this.license = data.license || "",
    this.image = data.image || "";
    this.birthday = data.birthday || '', 
    this.role = data.role || null;
  }

  toFormData() {
    const formData = new FormData();
    formData.append("Name", this.name);
    formData.append("PhoneNumber", this.phonenumber);
    if (this.cic !== "") {
      formData.append("CIC", this.cic);
    }
    if (this.license !== "") {
      formData.append("License", this.license);
    }
    if (this.image !== "") {
      formData.append("Image", this.image);
    }
    formData.append("Birthday", this.birthday);
    return formData;
  }
}