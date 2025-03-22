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
                    <input v-model="amenity.name" id="amenityName" type="text" class="form-control" required />
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
import AmenityService from '../../../../Service/api/AmenityService';
import AmenityDTO from '../../../../DTOs/AmenityDto';
export default {

    data() {
        return {
            amenity: new AmenityDTO(),
            update: false,
        };
    },
    methods: {
        async getAmenityById() {
            const id = this.$route.params.id;
            try {
                const result = await AmenityService.getAmenityById(id);
                this.amenity = result.data;
                console.log('Dữ liệu đã được lấy thành công:', result.data);
            } catch (error) {
                alert('Có lỗi xảy ra: ' + error.message);
            }
        },
        handleFileUpload(event) {
            // Lưu trữ file được tải lên
            this.amenity.iconImage = event.target.files[0];
            this.update = true;
        },

        async Edit() {
            try {
                const id = this.$route.params.id;
                const result = await AmenityService.UpdateAmenity({
                    id: parseInt(id),
                    name: this.amenity.name,
                    iconImage: this.amenity.iconImage
                }, id);
                console.log('Dữ liệu đã được gửi thành công:', result);
                this.$router.push({ name: 'admin-amenity' });
                alert('Tiện nghi đã được cập nhật thành công.');
            } catch (error) {
                alert('Có lỗi xảy ra: ' + error.message);
            }
        },

    },
    created() {
        this.getAmenityById();

    }
}
</script>

<style></style>
