<template>
    <div class="update">
        <div class="img">
            <label>
                <label class="image-container">
                    <img class="imgdd" :src="'http://localhost:5027/' + userform.image" alt="img">
                </label>
                <div style="margin-left: 25px;">Ảnh đại diện</div>
            </label>
        </div>
        <form @submit.prevent="submitForm1" enctype="multipart/form-data">
            <div class="form-group">
                <label for="amenityName" class="control-label">Tên tài xế 
                    <img v-if="userform.name !== ''" width="24" height="24" src="https://img.icons8.com/color/48/checked--v1.png" alt="checked--v1"/>
                    <img v-else width="24" height="24" src="https://img.icons8.com/color/48/cancel--v1.png" alt="cancel--v1"/>                
                </label>
                <input v-model="userform.name" id="amenityName" type="text" class="form-control" required />
            </div>
            <div class="form-group">
                <label for="iconImage" class="control-label">Giấy phép lái xe 
                    <img v-if="userform.license !== null" width="24" height="24" src="https://img.icons8.com/color/48/checked--v1.png" alt="checked--v1"/>
                    <img v-else width="24" height="24" src="https://img.icons8.com/color/48/cancel--v1.png" alt="cancel--v1"/>
                </label>
                <input @change="handleFileUpload" id="iconLicense" type="file" class="form-control" accept="image/*" />
            </div>
            <div class="form-group">
                <label for="iconImage" class="control-label">CCCD 
                    <img v-if="userform.cic !== null" width="24" height="24" src="https://img.icons8.com/color/48/checked--v1.png" alt="checked--v1"/>
                    <img v-else width="24" height="24" src="https://img.icons8.com/color/48/cancel--v1.png" alt="cancel--v1"/>
                </label>
                <input @change="handleFileUpload01" id="iconCIC" type="file" class="form-control" accept="image/*" />
            </div>
            <div class="form-group">
                <label for="iconImage" class="control-label">Ảnh đại diện 
                    <img v-if="userform.image !== null" width="24" height="24" src="https://img.icons8.com/color/48/checked--v1.png" alt="checked--v1"/>
                    <img v-else width="24" height="24" src="https://img.icons8.com/color/48/cancel--v1.png" alt="cancel--v1"/>
                </label>
                <input @change="handleFileUpload02" id="iconImage" type="file" class="form-control" accept="image/*" />
            </div>
            <div class="form-group">
                <label for="amenityName" class="control-label">Ngày sinh 
                    <img v-if="userform.birthday !== null" width="24" height="24" src="https://img.icons8.com/color/48/checked--v1.png" alt="checked--v1"/>
                    <img v-else width="24" height="24" src="https://img.icons8.com/color/48/cancel--v1.png" alt="cancel--v1"/>
                </label>
                <input v-model="userform.birthday" id="amenityName" type="datetime-local" class="form-control"
                    required />
            </div>
            <div class="form-group">
                <label for="amenityName" class="control-label">Số điện thoại
                    <img v-if="userform.phoneNumber !== null" width="24" height="24" src="https://img.icons8.com/color/48/checked--v1.png" alt="checked--v1"/>
                    <img v-else width="24" height="24" src="https://img.icons8.com/color/48/cancel--v1.png" alt="cancel--v1"/>
                </label>
                <input type="tel" v-model="userform.phoneNumber" pattern="[0-9]{10,11}" maxlength="11" required class="form-control">
            </div>
            <div class="form-group">
                <input type="submit" value="Cập nhật" class="btn btn-primary" />
            </div>
        </form>
    </div>
</template>

<script>
import axios from 'axios';
import UserVM from '../../../../Model/UserVM';
import AuthenticationService from '../../../../Service/api/AuthenticationService';
import UserDTO from '../../../../DTOs/UserDto';
export default {

    data() {
        return {
            userform: new UserVM(),
            userDTO: new UserDTO(),
            name: "",
            license: false,
            cic: false,
            image: false,
            birthday: null,
            phone: "",
        }
    },
    methods: {
        async submitForm1() {
            if (this.license == false || this.image == false || this.cic == false) {
                if (this.license == false) {
                    this.userform.license = '';
                }
                if (this.image == false) {
                    this.userform.image = '';
                }
                if (this.cic == false) {
                    this.userform.cic = '';
                }
            }
            try {
                this.userDTO = {
                    name : this.userform.name,
                    birthday : this.userform.birthday.toString(),
                    phoneNumber : this.userform.phoneNumber,
                    license : this.userform.license,
                    cic : this.userform.cic,
                    image : this.userform.image
                };
                const response = await AuthenticationService.UpdateComfirmDriver(this.userDTO);
                console.log("Response khi update: ",response);
                if(response.status == 200){
                    console.log("reponse from Update:", response);
                    this.$emit('Close');
                }
            }
            catch (error) {
                console.log(error);
            }

        },
        handleFileUpload(event) {
            // Lưu trữ file được tải lên
            this.userform.license = event.target.files[0];
            this.license = true;
        },
        handleFileUpload01(event) {
            // Lưu trữ file được tải lên
            this.userform.cic = event.target.files[0];
            this.cic = true;
        },
        handleFileUpload02(event) {
            // Lưu trữ file được tải lên
            this.userform.image = event.target.files[0];
            this.image = true;
        },
        async getUser() {
            const response = await AuthenticationService.getUser();
            this.userform = response;
            // this.phone = this.userform.phoneNumber;
            // this.name = this.userform.name;
            // this.birthday = this.userform.birthday;
        }
    },
    created() {
        this.getUser();
    },

}
</script>

<style>
    .update{
        margin-top: -20px;
    }
    .image-container {
    width: 150px;
    height: 150px;
    overflow: hidden;
}

    .image-container img {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }
    .image-container .imgdd{
        border-radius: 50%;
    }
    .img{
        display: flex;
        justify-content: space-between;
    }
</style>