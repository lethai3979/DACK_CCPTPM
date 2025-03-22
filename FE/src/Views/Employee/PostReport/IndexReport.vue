<template>

    <h1>Danh sách bài đăng vi phạm</h1>


    <table class="table">
        <thead>
            <tr>
                <th>
                    Tên
                </th>
                <th>
                    Hình ảnh
                </th>
                <th>
                   Nội dung báo cáo
                </th>
                <th>Trạng thái</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in reports" :key="item.id">
                <td>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'user-post-detail', params: { id: item.id } }">
                            <button>
                             {{ item.post.name }}
                            </button>
                        </router-link>
                    </li>
                </td>
                <td>
                    <img :src="'http://localhost:5027/' +item.post.image" alt="hinhanh.jpg" width="200px" />
                </td>
                <td>
                    {{ item.content }}
                </td>
                <td>
                    <button @click="Submit(item.id, true)">
                        Khóa bài viết
                    </button>
                    <button @click="Submit(item.id, false)">
                        Bỏ qua
                    </button>
                </td>
                
            </tr>

        </tbody>
    </table>
   
</template>

<script>
import axios from 'axios';
import PostVM from '../../../Model/PostVM';
import ReportVM from '../../../Model/ReportVM';
import ReportService from '../../../Service/api/ReportService';
export default {
    data() {
        return {
            posts: [new PostVM()],
            reports: [new ReportVM()]
        }
    },
    methods: {
        
        async getPost() {
            try {
                const response = await ReportService.getAllReport();
                
                    console.log("Report post: ",response);
                    this.reports = response.data;

            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
            }
        },
        async Submit(id,bien) {
            const response = await ReportService.SubmitReport(id,bien);
            console.log("Response submit report: ",response);
            if(response.ok){
                this.getPost();
            }
        }
    },
    mounted() {
        this.getPost();
    },
    created() {
        this.getPost();
    }
}
</script>


<style></style>

