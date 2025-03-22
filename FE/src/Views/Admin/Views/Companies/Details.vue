<template>

    <h1>Chi tiết</h1>

    <div>
        <h4>Loại xe</h4>
        <hr />
        <dl class="row" v-if="company != null">
            <dt class="col-sm-2">
                Tên
            </dt>
            <dd class="col-sm-10">
                {{ company.name }}
            </dd>
            <dt class="col-sm-2">
                Tên loại xe:
            </dt>
            <dd class="col-sm-10" v-for="item in company.carTypeDetail" :key="item.id">

                <div>{{ item.carTypeName }}</div>

            </dd>
            <dt class="col-sm-2">
                Mã người tạo
            </dt>
            <dd class="col-sm-10">
                {{ company.createdById }}
            </dd>
            <dt class="col-sm-2">
                Ngày tạo
            </dt>
            <dd class="col-sm-10">
                {{ company.createdOn }}
            </dd>
            <dt class="col-sm-2">
                Mã người cập nhật
            </dt>
            <dd class="col-sm-10">
                {{ company.modifiedById }}
            </dd>
            <dt class="col-sm-2">
                Ngày cập nhật
            </dt>
            <dd class="col-sm-10">
                {{ company.modifiedOn }}
            </dd>
            <dt class="col-sm-2">
                Ẩn
            </dt>
            <dd class="col-sm-10">
                {{ company.isDeleted }}
            </dd>
        </dl>
    </div>
    <div>
        <a asp-action="Edit" asp-route-id="@Model?.Id">Chỉnh sửa</a> |
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

</template>
<script>
import axios from 'axios';
import CompanyService from '../../../../Service/api/CompanyService';
import CompanyVM from '../../../../Model/CompanyVM';
export default {
    data() {
        return {
            company: new CompanyVM(),
        };
    },
    created() {
        this.getCompaniesById();
    },
    methods: {
        async getCompaniesById() {
            const id = this.$route.params.id;
            try {
                const res_company = await CompanyService.getCompanyById(id);
                console.log("company: ", res_company.data);
                this.company = res_company.data;
            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
            }
        }
    },
}
</script>

<style></style>