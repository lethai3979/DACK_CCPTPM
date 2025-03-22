<template>
    <div>
        <h1>Chỉnh sửa</h1>

        <h4>Hãng xe</h4>
        <hr />
        <div class="row" v-if="company != null">
            <div class="col-md-4">
                <form @submit.prevent="Edit" enctype="multipart/form-data">
                    <div class="form-group">
                        <label class="control-label">Tên</label>
                        <input v-model="company.name" class="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="iconCompany" class="control-label">Logo hãng xe</label>
                        <input @change="handleFileUpload" id="iconCompany" type="file" class="form-control"
                            accept="image/*" />
                    </div>
                    <div class="form-group">
                        <label class="control-label">Hãng xe:</label>
                        <br />

                        <div v-for="cartype in carTypes" :key="cartype.id" class="form-check">
                            <input type="checkbox" :value="cartype.id" v-model="selectedCartypes"
                                class="form-check-input" :checked="selectedCartypes.includes(cartype.id)" />
                            <label class="form-check-label">{{ cartype.name }}</label>
                        </div>
                    </div>

                    <div class="form-group form-check">
                        <label class="form-check-label">
                            <input type="checkbox" v-model="company.isDeleted" class="form-check-input" /> Ẩn
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
import CompanyVM from '../../../../Model/CompanyVM';
import CompanyService from '../../../../Service/api/CompanyService';
import { inject,ref } from 'vue';
export default {
    setup() {
        const carTypes = inject('carTypes', ref([]));
        return { carTypes }
    },
    data() {
        return {
            company: new CompanyVM(),
            selectedCartypes: [],
            iconCompany:null
        };
    },
    created() {       
        this.getCompanyBy();      
    },
    methods: {

        async getCompanyBy() {
            const id = this.$route.params.id;
            try {
                const res_company = await CompanyService.getCompanyById(id);
                console.log("company: ", res_company.data);
                this.company = res_company.data;
                this.selectedCartypes = this.company.carTypeDetail.map(cartype => cartype.carTypeId);
            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
            }
        },
        handleFileUpload(event) {
            this.iconCompany = event.target.files[0];
        },
        // Hàm Edit để cập nhật cartype
        async Edit() {
            if (this.company.name == "" || this.company.iconImage == null) {
                alert('Vui lòng điền đủ thông tin');
                return;
            }
            try {
                // Gửi yêu cầu PUT để cập nhật dữ liệu
                const response = await CompanyService.UpdateCompany({
                    id: this.company.id,
                    name: this.company.name,
                    iconImage: this.iconCompany,
                    carTypeIds: this.selectedCartypes
                })
                console.log(response);
                alert('Cập nhật thành công!');
                this.$router.push("/admin/company");
            } catch (error) {
                console.error('Lỗi khi cập nhật dữ liệu:', error);
            }
        },
    },
};
</script>
