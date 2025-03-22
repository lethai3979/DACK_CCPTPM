<template>

    <h1>Danh sách bài đăng</h1>


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
                    Mô tả
                </th>
                <th>
                    Địa chỉ nhận
                </th>
                <th>
                    Có tài xế
                </th>
                <th>
                    Giá
                </th>
                <th>
                    Hộp số
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in posts" :key="item.id">
                <td>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'user-post-detail', params: { id: item.id } }">
                            <button>
                             {{ item.name }}
                            </button>
                        </router-link>
                    </li>
                </td>
                <td>
                    <img :src="'http://localhost:5027/' +item.image" alt="hinhanh.jpg" width="200px" />
                </td>
                <td>
                    {{ item.description }}

                </td>
                <td>
                    {{ item.rentLocation }}

                </td>
                <td>
                    {{ item.hasDriver }}

                </td>
                <td>
                    {{ item.price }}
                </td>
                <td>
                    <text v-if="item.Gear">Số tự động</text>
                    <text v-else>Số sàn</text>
                </td>
                
            </tr>

        </tbody>
    </table>
   
</template>

<script>
import axios from 'axios';
import PostService from '../../../../Service/api/PostService';
import PostVM from '../../../../Model/PostVM';
export default {
    data() {
        return {
            posts: [new PostVM()],
         
        }
    },
    methods: {
        
        async getPost() {
            try {
                const response = await PostService.getAllPost();
                if (response != null) {
                    console.log(response);
                    this.posts = response.data.listPost;
                    console.log(this.posts);
                }

            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
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

