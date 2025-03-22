<template>
    <div>
        <h1>Chỉnh sửa</h1>

        <h4>Loại xe</h4>
        <hr />
        <div class="row" v-if="cartype != null">
            <div class="col-md-4" >
                <form @submit.prevent="Edit" enctype="multipart/form-data">
                    <div class="form-group">
                        <label class="control-label">Tên</label>
                        <input v-model="cartype.name" class="form-control" />
                    </div>

                    <div class="form-group">
                        <label class="control-label">Hãng xe:</label>
                        <br />
                        <div v-for="company in companies" :key="company.id" class="form-check">
                            <input type="checkbox" :value="company.id" v-model="selectedCompanies"
                                class="form-check-input" :checked="selectedCompanies.includes(company.id)" />
                            <label class="form-check-label">{{ company.name }}</label>
                        </div>
                    </div>

                    <div class="form-group form-check">
                        <label class="form-check-label">
                            <input type="checkbox" v-model="cartype.isDeleted" class="form-check-input" /> Ẩn
                        </label>
                    </div>

                    <div class="form-group">
                        <input type="submit" value="Lưu" class="btn btn-primary" />
                    </div>
                </form>
            </div>
        </div>

        <div>
            <a href="javascript:history.go(-1);">Quay lại</a>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { inject,ref } from 'vue';
import CarTypeService from '../../../../Service/api/CarTypeService';
import CarTypeVM from '../../../../Model/CartypeVM';
import CarTypeDTO from '../../../../DTOs/CartypeDto';
export default {
    setup() {
        const companies = inject('companies', ref([]));
        return { companies }
    },
    data() {
        return {                      
            cartype: new CarTypeVM(),
            selectedCompanies:[]
        };
    },
    created() {
        this.getCartypeById();      // Lấy chi tiết loại xe
    },
    methods: {
        async getCartypeById() {
            const id = this.$route.params.id;
            try {
                const res_cartype = await CarTypeService.getCartypeById(id);
                console.log("Cartype: ",res_cartype.data);
                this.cartype = res_cartype.data;
                this.selectedCompanies = this.cartype.carTypeDetail.map(company => company.companyId);
            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
            }
        },
        async Edit() {
            const id = this.$route.params.id;
            if (this.cartype.name == "" || this.cartype.companyIds == []) {
                alert('Vui lòng điền đủ thông tin');
                return;
            }
            try {
                // Tạo payload để gửi dữ liệu lên server
                const updateCarType =({
                    id: id,
                    name: this.cartype.name,
                    companyIds:this.selectedCompanies,
                });
                console.log(updateCarType);
                // Gửi yêu cầu PUT để cập nhật dữ liệu
                const response = await CarTypeService.UpdateCarType(updateCarType);
                console.log(response);
                alert('Cập nhật thành công!');
                this.$router.push("/cartype");
            } catch (error) {
                console.error('Lỗi khi cập nhật dữ liệu:', error);
            }
        },
    },
};
</script>

<style>
/* Thêm các style cần thiết */
</style>