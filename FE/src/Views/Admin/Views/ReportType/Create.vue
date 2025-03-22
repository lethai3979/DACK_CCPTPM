<template>
    <div style="margin-bottom:20px;">
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

    <h1>Thêm loại báo cáo</h1>
    <h4>Điền thông tin</h4>
    <hr />
    <div class="row">
        <div class="col-md-4">
            <form @submit.prevent="submitForm" enctype="multipart/form-data">
                <div class="form-group">
                    <label for="reportName" class="control-label">Tên loại báo cáo</label>
                    <input v-model="reportType.name" id="reportName"  type="text" class="form-control" required />
                </div>
                <div class="form-group">
                    <label for="reportPoint" class="control-label">Điểm bị trừ</label>
                    <input v-model="reportType.reportPoint" min="0" max="5" id="reportPoint" type="number" class="form-control" required />
                </div>
                <div class="form-group">
                    <input type="submit" value="Tạo" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>
</template>

<script>
import ReportTypeDTO from '../../../../DTOs/ReportType';
import ReportTypeService from '../../../../Service/api/ReportTypeService';
export default {
    data() {
        return {
            reportType: new ReportTypeDTO(), // Tên tiện nghi
        };
    },
    methods: {
        async submitForm() {
            if (!this.reportType.name || this.reportType.reportPoint == 0) {
                alert('Vui lòng điền đủ thông tin.');
                return;
            }
            console.log(this.reportType);
            try {
                // Thực hiện gửi yêu cầu POST lên API
                const response = await ReportTypeService.AddReportType(this.reportType);
                if (response.success) {
                    this.$router.push({ name: 'admin-reportType' });
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
    }
}
</script>

<style>
/* Style tùy chỉnh nếu cần */
</style>
