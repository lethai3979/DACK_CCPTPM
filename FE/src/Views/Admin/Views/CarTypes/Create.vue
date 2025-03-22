<template>
    <div style="margin-bottom:20px;">
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

    <h1>Thêm loại xe</h1>
    <h4>Điền thông tin</h4>
    <hr />
    <div >
        <div >
            <form @submit.prevent="submitForm" enctype="multipart/form-data">
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                <div class="form-group">
                    <label :for="cartype.name" class="control-label">Tên loại xe</label>
                    <input v-model="cartype.name" id="cartypeName" type="text" class="form-control" required />
                </div>
                <div class="checkbox-list">
                    Hãng xe:
                    <ul v-if="companies != null">
                        <li v-for="item in companies" :key="item.id">
                            <div class="form-check">
                                <input class="form-check-input car-type-checkbox" type="checkbox" :value="item.id"
                                    v-model="cartype.companyIds">
                                <label class="form-check-label">
                                    {{ item.name }}
                                </label>
                            </div>
                        </li>
                    </ul>
                    <ul v-else>
                        Chưa có hãng xe nào.
                    </ul>
                </div>
                <div class="form-group">
                    <input type="submit" value="Tạo" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { inject,ref } from 'vue';
import CarTypeService from '../../../../Service/api/CarTypeService';
import CarTypeDTO from '../../../../DTOs/CartypeDto';
export default {
    setup() {
        const companies = inject('companies', ref([]));
        return { companies }
    },
    data() {
        return {
            cartype: new CarTypeDTO(),
        };
    },
    methods: {
        async submitForm() {
            if (this.cartype.name == "" || this.cartype.companyIds == []) {
                alert('Vui lòng điền đủ thông tin');
                return;
            }
            try {
                const result = await CarTypeService.AddCartype(this.cartype);
                console.log(result);
                if (result.success) {
                    this.$router.push({ name: 'admin-cartype' });
                } else {
                    const errorText = await response.text();
                    console.error('Có lỗi xảy ra khi gửi dữ liệu:', response.statusText, errorText);
                    alert('Có lỗi xảy ra: ' + response.statusText);
                }
            } catch (error) {
                console.error('Lỗi kết nối:', error);
                alert('Lỗi kết nối hoặc lỗi khác. Vui lòng thử lại sau.');
            }
        }

    }
}
</script>

<style>
/* Style tùy chỉnh nếu cần */
</style>
