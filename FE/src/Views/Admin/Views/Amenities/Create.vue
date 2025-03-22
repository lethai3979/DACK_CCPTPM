<template>
    <div style="margin-bottom:20px;">
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

    <h1>Thêm tiện nghi</h1>
    <h4>Điền thông tin</h4>
    <hr />
    <div >
        <div >
            <form @submit.prevent="submitForm" enctype="multipart/form-data">
                <div class="form-group">
                    <label for="amenityName" class="control-label">Tên tiện nghi</label>
                    <input v-model="amenityName" id="amenityName" type="text" class="form-control" required />
                </div>
                <div class="form-group">
                    <label for="iconImage" class="control-label">Icon</label>
                    <input @change="handleFileUpload" id="iconImage" type="file" class="form-control" accept="image/*"
                        required />
                </div>
                <div class="form-group">
                    <input type="submit" value="Tạo" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>
</template>

<script>
import AmenityService from '../../../../Service/api/AmenityService';
import AmenityDTO from '../../../../DTOs/AmenityDto';
export default {
    data() {
        return {
            amenityName: '', // Tên tiện nghi
            iconImage: null, // Dữ liệu hình ảnh

        };
    },
    methods: {
        handleFileUpload(event) {
            // Lưu trữ file được tải lên
            this.iconImage = event.target.files[0];
        },
        async submitForm() {
            if (!this.iconImage || !this.amenityName) {
                alert('Vui lòng điền đủ thông tin và chọn ảnh.');
                return;
            }
            try {
                // Thực hiện gửi yêu cầu POST lên API
                const result = await AmenityService.addAmenity({
                    name: this.amenityName,
                    iconImage: this.iconImage
                });
                console.log('Dữ liệu đã được gửi thành công:', result);
                this.$router.push({ name: 'admin-amenity' });
                // alert('Tiện nghi đã được tạo thành công.');
            } catch (error) {
                // alert('Có lỗi xảy ra: ' + error.message);
            }
        }
    }
}
</script>

<style>
/* Style tùy chỉnh nếu cần */
</style>
