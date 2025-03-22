<template>
    <div style="padding-bottom: 40px;">
        <div style="font-size:20px;background-color:var(--color-green2);padding:20px 30px;border-radius:15px;">
            <div style="font-size:20px;color:white;">
                <h1>Tạo bài đăng</h1>
                <h4>Vui lòng điền đầy đủ vào mẫu</h4>
            </div>
        </div>
        <hr />
        <form @submit.prevent="submitPost">
            <div class="form-group">
                <label for="name">Tên xe</label>
                <input v-model="post.name" maxlength="20" type="text" class="form-control" id="name" required />
            </div>


            <!-- Company Selection -->

            <div class="form-group">
                <label for="companyId">Hãng xe</label>
                <select v-model="post.companyId" @change="fetchCarTypes" class="form-control" id="companyId" required>
                    <option value="null">-- Select Company --</option>
                    <option v-for="company in companies" :key="company.id" :value="company.id">
                        {{ company.name }}
                    </option>
                </select>
            </div>


            <!-- Car Type Selection -->
            <div class="form-group" >
                <label for="carTypeId">Loại xe</label>
                <select v-if="carTypes == null" v-model="post.carTypeId" class="form-control" :disabled="openCartype == false" id="carTypeId" required>
                    <option value="null">-- Không có loại xe nào phù hợp với hãng xe --</option>
                </select>
                <select v-else v-model="post.carTypeId" :disabled="openCartype == false" class="form-control" id="carTypeId" required>
                    <!-- <option value="null">-- Select Car Type --</option> -->
                    <option v-for="carType in carTypes" :key="carType" :value="carType.carTypeId">
                        {{ carType.carTypeName }}
                    </option>
                </select>
            </div>


            <!-- Main Image Upload -->
            <div class="form-group">
                <label for="image">Ảnh chính</label>
                <input type="file" @change="handleMainImage" class="form-control" id="image" accept="image/*"
                    required />
            </div>

            <!-- Multiple Images Upload -->
            <div class="form-group">
                <label for="images">Ảnh phụ (cần 3 ảnh)</label>
                <input type="file" @change="handleMultipleImages" class="form-control" id="images" multiple
                    accept="image/*" />
            </div>

            <!-- Description -->
            <div class="form-group">
                <label for="description">Mô tả</label>
                <textarea v-model="post.description" class="form-control" id="description" required></textarea>
            </div>

            <!-- Seat -->
            <div class="form-group">
                <label for="seat">Chổ ngồi</label>
                <input v-model="post.seat" type="number" min="2" class="form-control" id="seat" required />
            </div>

            <!-- Rent Location -->
            <div class="form-group">
                <label for="rentLocation">Vị trí xe</label>
                <input v-model="post.rentLocation" type="text" class="form-control" id="rentLocation" required />
            </div>

            <!-- Amenities -->

            <div class="form-group">
                <label>Tiện nghi</label>
                <div v-for="amenity in amenities" :key="amenity.id" class="form-check">
                    <input type="checkbox" :value="amenity.id" v-model="post.amenitiesIds"
                        class="form-check-input" />
                    <label class="form-check-label">{{ amenity.name }}</label>
                </div>
            </div>

            <!-- Has Driver -->
            <div class="form-group">
                <label>Xe có tài xế:</label>
                <input type="checkbox" v-model="post.hasDriver" />
            </div>

            <!-- Price -->
            <div class="form-group">
                <label for="price">Giá (theo giờ)</label>
                <input v-model="post.pricePerHour" min="5" type="number" class="form-control" id="price" required />
            </div>
            <div class="form-group">
                <label for="price">Giá (theo ngày)</label>
                <input v-model="post.pricePerDay" min="50" type="number" class="form-control" id="price" required />
            </div>

            <!-- Gear -->
            <div class="form-group">
                <label>Hộp số</label>
                <div>
                    <input type="radio" v-model="post.gear" value="true" /> Tự động
                    <input type="radio" v-model="post.gear" value="false" /> Số sàn
                </div>
            </div>

            <!-- Fuel -->
            <div class="form-group">
                <label for="fuel">Loại nhiên liệu</label>
                <select v-model="post.fuel" class="form-control" id="fuel" required>
                    <option value="Gasoline">Xăng</option>
                    <option value="Diesel">Dầu</option>
                    <option value="Electric">Điện</option>
                </select>
            </div>

            <!-- Fuel Consumption -->
            <div class="form-group">
                <label for="fuelConsumed">Nhiên liệu tiêu hao (liters/100km)</label>
                <input v-model="post.fuelConsumed" type="number" class="form-control" id="fuelConsumed" required />
            </div>

            <!-- Submit -->
            <button type="submit" class="btn btn-primary">Tạo bài đăng</button>
        </form>
    </div>
</template>

<script>
import { inject, ref } from 'vue';
import PostDTO from '../../../../DTOs/PostDto';
import PostService from '../../../../Service/api/PostService';
export default {
    setup() {
        const companies = inject('companies', ref([]));
        const amenities = inject('amenities', ref([]));
        return { companies,amenities }
    },
    data() {
        return {
            post: new PostDTO(),
            carTypes: [],
            openCartype: false
        };
    },
    methods: {
        async fetchCarTypes() {
            this.openCartype = true;
            if (this.post.companyId && this.post.companyId !== 'null') {
                try {
                    const res_company = await PostService.getCarTypies(this.post.companyId);
                    console.log(res_company);
                    this.carTypes = res_company.data.carTypeDetail; // Lưu danh sách kiểu xe của công ty
                    if (!this.carTypes || this.carTypes.length === 0) {
                        this.post.carTypeId = null;
                    }
                } catch (error) {
                    console.error('Lỗi khi lấy danh sách kiểu xe:', error);
                }
            } else {
                this.carTypes = null; 
            }
        },


        // Handle main image selection
        handleMainImage(event) {
            const file = event.target.files[0];
            if (file) {
                this.post.image = file;
            }
        },

        // Handle multiple image selection (up to 3 images)
        handleMultipleImages(event) {
            const files = Array.from(event.target.files);
            if (files.length != 3) {
                alert('You can select up to 3 images.');
                event.target.value = ''; // Reset file input
                return;
            }
            this.post.imagesList = files;
            console.log(this.post.imagesList);
        },
        async submitPost() {
            try {
                // Thực hiện gửi yêu cầu POST lên API
                const response = await PostService.AddPost(this.post);
                console.log(response);
                if (response.ok) {
                    const result = await response.json();
                    console.log('Dữ liệu đã được gửi thành công:', result);
                    this.$router.push({ name: 'user-profile-mycar' });

                } else {
                    // Nếu có lỗi xảy ra từ API
                    const errorText = await response.text();
                    console.error('Có lỗi xảy ra khi gửi dữ liệu:', response.statusText, errorText);
                    alert('Có lỗi xảy ra: ' + response.statusText);
                }
            } catch (error) {
                // Lỗi kết nối hoặc các lỗi khác
                console.error('Lỗi kết nối:', error);
                alert('Lỗi kết nối hoặc lỗi khác. Vui lòng thử lại sau.');
            }




        }
    },
    mounted() {
        
    },
};
</script>

<style>
.form-group {
    margin-bottom: 15px;
}
</style>