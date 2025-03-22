<template>
    <div>
        <h1>Chỉnh sửa thông tin</h1>
        <hr />
        <form @submit.prevent="submitForm" v-if="post != null">
            <div class="form-group">
                <label for="name">Tên</label>
                <input v-model="post.name" id="name" class="form-control" />
            </div>

            <div class="form-group">
                <label for="companyId">Company</label>
                <select v-model="post.companyId" @change="fetchCarTypes" class="form-control" id="companyId" required>
                    <option value="null">-- Select Company --</option>
                    <option v-for="company in companies" :key="company.id" :value="company.id">
                        {{ company.name }}
                    </option>
                </select>
            </div>


            <!-- Car Type Selection -->
            <div class="form-group">
                <label for="carTypeId">Car Type</label>
                <select v-model="post.carTypeId" class="form-control" id="carTypeId" required>
                    <!-- <option :value="post.carTypeId">-- Select Car Type --</option> -->
                    <option v-for="carType in carTypes" :key="carType" :value="carType.carTypeId">
                        {{ carType.carTypeName }}
                    </option>
                </select>
            </div>


            <div class="form-group">
                <label for="image">Hình ảnh</label>
                <div v-if="post.image">
                    <button @click="removeImage">X</button>
                    <img :src="post.image" alt="Image" style="max-width: 200px; margin-top: 10px;" />
                </div>
                <input type="file" @change="handleMainImage" class="form-control" />
            </div>
            <div class="form-group">
                <label for="image">Danh sách hình ảnh phụ</label>
                <div v-if="post.images">
                    <button @click="removeImages">X</button>
                    <ul v-for="item in post.images" :key="item">
                        <li><img :src="item.image" alt="Image" style="max-width: 200px; margin-top: 10px;" /></li>
                    </ul>

                </div>
                <input type="file" @change="handleMainImage" class="form-control" />
            </div>


            <div class="form-group">
                <label for="description">Mô tả</label>
                <input v-model="post.description" id="description" class="form-control" />
            </div>

            <div class="form-group">
                <label for="seat">Chỗ ngồi</label>
                <input v-model="post.seat" id="seat" class="form-control" type="number" />
            </div>

            <div class="form-group">
                <label for="rentLocation">Địa chỉ nhận</label>
                <input v-model="post.rentLocation" id="rentLocation" class="form-control" />
            </div>

            <div class="form-group">
                <label>Các tiện ích:</label>
                <div v-for="amenity in amenities" :key="amenity.id" class="form-check">
                    <input type="checkbox" :value="amenity.id" v-model="selectedAmenities" class="form-check-input"
                        :checked="selectedAmenities.includes(amenity.id)" />
                    <label class="form-check-label">{{ amenity.name }}</label>
                </div>
            </div>




            <div class="form-group form-check">
                <label for="hasDriver">Có tài xế</label>
                <input v-model="post.hasDriver" type="checkbox" id="hasDriver" class="form-check-input" />
            </div>

            <div class="form-group">
                <label for="price">Giá thuê (đồng/giờ)</label>
                <input v-model="post.pricePerHour" id="price" class="form-control" type="number" />
            </div>
            <div class="form-group">
                <label for="price">Giá thuê (đồng/ngày)</label>
                <input v-model="post.pricePerDay" id="price" class="form-control" type="number" />
            </div>

            <div class="form-group">
                <label>Hộp số</label>
                <div class="form-check">
                    <input type="radio" v-model="post.gear" :value="true" id="automaticRadio"
                        class="form-check-input" />
                    <label for="automaticRadio" class="form-check-label">Số tự động</label>
                </div>
                <div class="form-check">
                    <input type="radio" v-model="post.gear" :value="false" id="manualRadio" class="form-check-input" />
                    <label for="manualRadio" class="form-check-label">Số sàn</label>
                </div>
            </div>

            <div class="form-group">
                <label for="fuel">Fuel Type</label>
                <select v-model="post.fuel" class="form-control" id="fuel" required>
                    <option value="Gasoline">Gasoline</option>
                    <option value="Diesel">Diesel</option>
                    <option value="Electric">Electric</option>
                </select>
            </div>

            <div class="form-group">
                <label for="fuelConsumed">Nhiên liệu tiêu thụ (lít/100km)</label>
                <input v-model="post.fuelConsumed" id="fuelConsumed" class="form-control" type="number" />
            </div>


            <div class="form-group">
                <button type="submit" class="btn btn-primary">Lưu thông tin</button>
            </div>
        </form>

        <div>
            <a href="javascript:history.go(-1);">Quay lại</a>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { errorMessages } from 'vue/compiler-sfc';
import { inject, ref } from 'vue';
import PostDTO from '../../../../DTOs/PostDto';
import PostService from '../../../../Service/api/PostService';
import PostVM from '../../../../Model/PostVM';
import el from 'vue-cal/dist/i18n/el.es.js';
export default {
    setup() {
        const companies = inject('companies', ref([]));
        const amenities = inject('amenities', ref([]));
        return { companies, amenities }
    },
    data() {

        return {
            post: new PostVM(),
            postDto: new PostDTO(),
            carTypes: [],
            selectedAmenities: [],
            errors: {},
            string: '',
            updateimg: false,
            updateimgs: false,
        };
    },
    created() {
        this.fetchData();
    },
    methods: {
        async fetchData() {
            const id = this.$route.params.id;
            try {
                const response = await PostService.getPost(id);
                this.post = response.data; // Load post data từ server
                console.log(this.post);
                this.selectedAmenities = this.post.postAmenities.map(item => item.amenityId);
                this.fetchCarTypes();
            } catch (error) {
                console.error('Error fetching post details:', error);
            }
        },
        async fetchCarTypes() {
            if (this.post.companyId && this.post.companyId !== 'null') {
                try {
                    const res_company = await PostService.getCarTypies(this.post.companyId);
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
            this.updateimgs = true;
            this.post.images = files;
            console.log(this.post.images);
        },

        removeImage() {
            this.post.image = null;
            this.updateimg = true;
        },
        removeImages() {
            this.post.images = [];
            this.updateimgs = true;
        },
        mapPostToPostDto() {
            // Tạo `postDto` từ các trường cần thiết của `post`
            this.postDto = {
                id: this.post.id,
                name: this.post.name,
                image: this.post.image,
                description: this.post.description,
                seat: this.post.seat,
                rentLocation: this.post.rentLocation,
                hasDriver: this.post.hasDriver,
                pricePerDay: this.post.pricePerDay,
                pricePerHour: this.post.pricePerHour,
                gear: this.post.gear,
                fuel: this.post.fuel,
                fuelConsumed: this.post.fuelConsumed,
                rideNumber: this.post.rideNumber,
                avgRating: this.post.avgRating,
                isAvailable: this.post.isAvailable,
                isDisabled: this.post.isDisabled,
                carTypeId: this.post.carTypeId,
                companyId: this.post.companyId,
                amenitiesIds: this.selectedAmenities
            };
        },
        
        async submitForm() {
            try {
                if (this.updateimg == true) {
                    if (this.post.image == null) {
                        alert('Điền ảnh vào cho bài viết nào');
                    }
                }
                else {
                    this.post.image = null;
                }
                if (this.updateimgs == true) {
                    if (this.post.images == []) {
                        alert('Điền danh sách ảnh vào cho bài viết nào');
                    }
                }
                else {
                    this.post.images = [];
                }
                const id = this.$route.params.id;
                this.mapPostToPostDto();
                try {
                    // Thực hiện gửi yêu cầu POST lên API
                    const response = await PostService.UpdatePost(this.postDto);
                    console.log(response);
                    if (response.statusText.statusCode == 200) {
                        const result = await response.json();
                        console.log('Dữ liệu đã được gửi thành công:', result);
                        this.$router.push({ name: '/customer' });

                    } else {
                        // Nếu có lỗi xảy ra từ API
                        const errorText = await response.text();
                        console.error('Có lỗi xảy ra khi gửi dữ liệu:', response.statusText, errorText);

                    }
                } catch (error) {
                    // Lỗi kết nối hoặc các lỗi khác
                    console.error('Lỗi kết nối:', error);

                }
                alert('Thông tin đã được lưu!');
            } catch (error) {
                console.error('Error submitting form:', error);
                if (error.response && error.response.data) {
                    this.errors = error.response.data.errors; // Hiển thị lỗi từ server
                }
            }
        },
    },
};
</script>

<style scoped>
.form-group {
    margin-bottom: 15px;
}
</style>
