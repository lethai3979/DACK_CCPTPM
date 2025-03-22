<template>

    <h1>Chỉnh sửa tiện nghi</h1>

    <hr />
    <div class="row">
        <div class="col-md-4">
            <h2>Ảnh của amenity {{ amenityName }}</h2>
            <img :src="iconImage" alt="">
            <form @submit.prevent="Edit" enctype="multipart/form-data">
                <div class="form-group">
                    <label for="amenityName" class="control-label">Tên tiện nghi</label>
                    <input v-model="amenityName" id="amenityName" type="text" class="form-control" required />
                </div>
                <div class="form-group">
                    <label for="iconImage" class="control-label">Icon</label>
                    <input @change="handleFileUpload" id="iconImage" type="file" class="form-control"
                        accept="image/*" />
                </div>

                <div class="form-group">
                    <input type="submit" value="Edit" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>

    <div>
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>


</template>

<script>
import axios from 'axios';
export default {

    data() {
        return {
            amenity: null,
            amenityName: null, // Tên tiện nghi
            iconImage: null, // Dữ liệu hình ảnh
            token: '',
            id: 0,
        };
    },
    methods: {
        async getAmenityById() {
            console.log(this.id);
            try {
                const response = await axios.get(`https://localhost:7265/api/Amenity/GetById/${this.id}`);
                console.log(response.data.data);
                this.amenity = response.data.data;
                this.amenityName = this.amenity.name;
                this.iconImage = this.amenity.iconImage;

                console.log(this.amenity.createdById);
            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
            }
        },
        handleFileUpload(event) {
            // Lưu trữ file được tải lên
            this.iconImage = event.target.files[0];
        },

        async Edit() {
            const formData = new FormData();
            console.log("ID : ", this.id);
            formData.append('Id', this.id);
            console.log(this.amenity);
            formData.append('Name', this.amenityName);
            formData.append('IconImage', this.iconImage);
            
            const token = sessionStorage.getItem('authToken');
            try {
                // Thực hiện gửi yêu cầu POST lên API
                const response = await fetch(`https://localhost:7265/api/admin/Amenity/Update/${this.id}`, {
                    method: 'PUT',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    },
                    body:   formData
                });

                if (response.ok) {
                console.log(response);
                this.$router.push({ name: 'admin-amenity' });
                }
            }
            catch (error) {
                console.error('Lỗi gửi dữ liệu:', error);
            }
        },

    },
    mounted() {
        this.getAmenityById();
        this.id = this.$route.params.id;
    },
    created() {
        this.getAmenityById();
        this.id = this.$route.params.id;
    }
}
</script>

<style></style>
