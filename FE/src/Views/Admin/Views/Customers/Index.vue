<template>

    <h1>Danh sách người dùng</h1>
    <table class="table">
        <thead>
            <tr>
                <th>
                    Khách hàng
                </th>
                <th>
                    Email
                </th>
                <th>
                    SĐT
                </th>
                <th>
                    Báo cáo
                </th>
                <th>
                    Ngày sinh
                </th>
                <th>
                    Thời gian khóa
                </th>
                <th>
                    Thao tác
                </th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="item in users" :key="item">
                <td>
                    <a @click="this.$router.push({ name: 'user-infor', params: { userId: item.id } })"><img
                            class="user-img-home" style="width: 50px;height: 50px;object-fit: cover;margin-right: 10px;" :src="`http://localhost:5027/` + item.image" alt="Image" />{{
                                item.name
                        }}</a>
                </td>
                <td>
                    {{ item.email }}
                </td>
                <td>
                    {{ item.phoneNumber }}
                </td>
                <td>
                    {{ item.reportPoint }}
                </td>
                <td>
                    {{ item.birthday }}
                </td>
                <td v-if="item.lockoutEnd > Date.now">
                    {{ item.lockoutEnd }}
                </td>
                <td v-else></td>
                <td v-if="item.lockoutEnd > Date.now">
                        <a @click="Submit(item.id)">Mở khóa</a>
                </td>
                
                <td v-else>
                    <a @click="Submit(item.id)">Khóa tài khoản</a>
                </td>

            </tr>
        </tbody>
    </table>
</template>

<script>
import UserVM from '../../../../Model/UserVM';
import axios from 'axios';

export default {


    data() {
        return {
            users: [new UserVM()]
        }
    },
    methods: {
        async getAllUser() {
            const token = sessionStorage.getItem("authToken");
            const response = await axios.get('http://localhost:5027/api/ManageUser/GetAllUser', {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            console.log(response);
            this.users = response.data.data;
        },
        async Submit(userId) {
            const token = sessionStorage.getItem("authToken");
            const response = await axios.put(`http://localhost:5027/api/ManageUser/LockUserAccount/${userId}`,userId, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            console.log("Response trả về report: ",response);
        }
    },

    async created() {
        await this.getAllUser();
    },
}
</script>

<style></style>
