<template>

    <div style="margin-bottom:20px;">
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

    <h1>Thêm mới hãng xe</h1>

    <hr />
    <div >
        <div >
            <form @submit.prevent="submitForm" enctype="multipart/form-data">
                <div class="form-group">
                    <label for="companyName" class="control-label">Tên hãng xe</label>
                    <input v-model="company.name" id="companyName" type="text" class="form-control" required />
                </div>
                <div class="form-group">
                    <label for="iconCompany" class="control-label">Logo hãng xe</label>
                    <input @change="handleFileUpload" id="iconCompany" type="file" class="form-control" accept="image/*"
                        required />
                </div>
                <div class="checkbox-list">
                    Loại xe:
                    <ul v-if="cartypes != null">
                        <li v-for="item in cartypes" :key="item.id">
                            <div class="form-check">
                                <input class="form-check-input car-type-checkbox" type="checkbox" :value="item.id"
                                    v-model="company.carTypeIds">
                                <label class="form-check-label">
                                    {{ item.name }}
                                </label>
                            </div>
                        </li>
                    </ul>
                    <ul v-else>
                        Chưa có loại xe nào.
                    </ul>
                </div>
                <div class="form-group">
                    <input type="submit" value="Tạo" class="btn btn-primary" />
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
import { inject, ref } from 'vue';
import CompanyDTO from '../../../../DTOs/CompanyDto';
import CompanyService from '../../../../Service/api/CompanyService';
export default {
    setup() {
        const cartypes = inject('carTypes', ref([]));
        return { cartypes }
    },
    data() {
        return {
            company: new CompanyDTO(),
        };
    },
    methods: {
        handleFileUpload(event) {
            // Lưu trữ file được tải lên
            this.company.iconImage = event.target.files[0];
        },
        async submitForm() {
            if (this.company.name == "" || this.company.iconImage == null) {
                alert('Vui lòng điền đủ thông tin');
                return;
            }
            try {
                const result = await CompanyService.AddCompany(this.company);
                this.$router.push({ name: 'admin-company' });
            } catch (error) {
                console.error('Lỗi kết nối:', error);
                alert('Lỗi kết nối hoặc lỗi khác. Vui lòng thử lại sau.');
            }
        }

    }
}
</script>

<style></style>
