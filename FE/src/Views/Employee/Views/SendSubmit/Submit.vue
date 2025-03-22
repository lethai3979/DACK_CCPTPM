<template>

    <h1>Danh sách xác nhận tài xế</h1>

    <table class="table">
        <thead>
            <tr>
                <th>
                    Tên
                </th>
                <th>
                    Ngày sinh
                </th>
                <th>
                    Số điện thoại
                </th>
                <th>Chức năng</th>
            </tr>
        </thead>

        <tbody v-if="drives.length > 0">
            <tr v-for="item in drives" :key="item.id">
                <td>
                    {{ item.name }}
                </td>
                <td>
                    {{ item.birthday }}
                </td>
                <td>
                    {{ item.phonenumber }}
                </td>
                <td>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'employee-driver-detail', params: { id: item.id } }">
                            <button>
                                Kiểm tra
                            </button>
                        </router-link>
                    </li>
                </td>
            </tr>
        </tbody>
        <tbody v-else>
            Danh sách trống
        </tbody>
    </table>

</template>
<script>
import axios from 'axios';
import { inject, ref } from 'vue';
export default {


    data() {
        return {
            drives: []
        }
    },
    methods: {
        async getAllDiver() {
            const token = sessionStorage.getItem("authToken");
            const response = await axios.get('http://localhost:5027/api/ManageUser/GetAllDriverSubmit', {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            console.log(response);
            this.drives = response.data.data;
        }
    },

    async created() {
        await this.getAllDiver();
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