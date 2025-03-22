<template>

    <h1>Chi tiết tiện nghi</h1>

    <div>
        <h4>Thông tin tiện nghi</h4>
        <hr />
        <dl class="row" v-if="reportType != null">
            <dt class="col-sm-2">
                Tên
            </dt>
            <dd class="col-sm-10">
                {{ reportType.name }}
            </dd>
            <dt class="col-sm-2">
                Icon
            </dt>
            <dd class="col-sm-10">
                {{ reportType.reportPoint }}
            </dd>
            <dt class="col-sm-2">
                Mã người tạo
            </dt>
            <dd class="col-sm-10">
                {{ reportType.createdById }}
            </dd>
            <dt class="col-sm-2">
                Ngày tạo
            </dt>
            <dd class="col-sm-10">
                {{ reportType.createdOn }}
            </dd>
            <dt class="col-sm-2">
                Mã người cập nhật
            </dt>
            <dd class="col-sm-10">
                {{ reportType.modifiedById }}
            </dd>
            <dt class="col-sm-2">
                Ngày cập nhật
            </dt>
            <dd class="col-sm-10">
                {{ reportType.modifiedOn }}
            </dd>
            <dt class="col-sm-2">
                Ẩn
            </dt>
            <dd class="col-sm-10">
                {{ reportType.isDeleted }}
            </dd>
        </dl>
        <h1 v-else>
            Loading.......
        </h1>
    </div>
    <div>
        <!-- <a asp-action="Edit" asp-route-id="@Model?.Id">Chỉnh sửa</a> | -->
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

</template>

<script>
import axios from 'axios';
import ReportTypeVM from '../../../../Model/ReportTypeVM';
import ReportTypeService from '../../../../Service/api/ReportTypeService';
export default {

    data() {
        return {
            reportType : new ReportTypeVM(),
        }
    },
    methods: {
        async getReportTypeById() {
            const id = this.$route.params.id;
            console.log(id);
            try {
                const response = await ReportTypeService.GetById(id);
                console.log(response);
                if (response.success) {
                    this.reportType = response.data;
                }
            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
            }
        }
    },
    created() {
        this.getReportTypeById();
    }
}
</script>

<style></style>
