<template>
    <h1>Danh sách bài đăng vi phạm</h1>

    <table class="table">
        <thead>
            <tr>
                <th>
                    Nội dung
                </th>
                <th>
                    Bài đăng
                </th>
                <th>
                    Người báo cáo
                </th>
                <th>
                    Ngày báo cáo
                </th>
                <th>
                    Trạng thái
                </th>
                <th>Chức năng</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in report" :key="item.id">
                <td>
                    {{ item.content }}
                </td>
                <td>
                    <a asp-area="Customer" asp-controller="Posts" asp-action="Details" asp-route-id="@item.PostId">{{
                        item.post.name }}</a>

                </td>
                <td>
                    <a asp-area="Admin" asp-controller="Customers" asp-action="Details"
                        asp-route-id="@item.CreatedById">Yêu cầu fix lỗi</a>
                </td>
                <td>
                    {{ item.createdOn }}
                    <!-- @Html.DisplayFor(modelItem => item.CreatedOn) -->
                </td>
                
                <td>
                    
                    <text v-if="item.isDeleted">Đã xử lí</text>
                  
                    <text v-else>Chưa xử lí</text>
                    
                </td>
                <td>
                    <a @click="SubmitReport(1, item.id)">Khóa bài đăng</a>
                    <a @click="SubmitReport(2, item.id)">Bỏ qua</a>
                </td>
            </tr>

        </tbody>
    </table>
</template>

<script>
import ReportService from '../../../../Service/api/ReportService';

export default {
    data() {
        return {
            report: null
        }
    },
    methods: {
        async getAllReport() {
            const response = await ReportService.getAllReport();
            console.log("Report all in index: ", response);
            if (response.success) {
                this.report = response.data;
            }
        },
        async SubmitReport(bien, item) {
            if (bien == 1) {
                const response = await ReportService.SubmitReport(item, true);
                console.log(response); 
                if (response.ok) {
                    this.getAllReport();
                }
            }
            if (bien == 2) {
                const response = await ReportService.SubmitReport(item, false);
                console.log(response); 
                if (response.ok) {
                    this.getAllReport();
                }
            }
        
        },
    },
    created() {
        this.getAllReport();
    },

}
</script>

<style>
     .table {
    width: 100%;
    border-collapse: collapse;
    margin-block: 15px;
}

    .table td {
        margin-left:15px;
        border: 1px solid #ddd;
        padding: 13px;
        text-align: left;
        border: 1px solid #ccc;
    }

    .table th {
        text-align:center;
        background-color: #ccffcc;
    }

    .table tr:nth-child(1) td {
        font-weight: bold;
        padding: 10px;
    }
tbody tr {
    border: 1px solid #ccc; 
}

    tbody tr td {
        padding: 10px; 
    }
a {
    text-decoration: none;
    color: black;
    margin-inline:15px;
} 
</style>
