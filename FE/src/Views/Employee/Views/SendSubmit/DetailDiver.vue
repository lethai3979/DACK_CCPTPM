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
                <th>
                    CIC
                </th>
                <th>
                    License
                </th>
                <th>Chức năng</th>
            </tr>
        </thead>

        <tbody v-if="drive != null">
            <tr>
                <td>
                    {{ drive.name }}
                </td>
                <td>
                    {{ drive.birthday }}
                </td>
                <td>
                    {{ drive.phoneNumber }}
                </td>
                <td>
                    <img width="300px" :src="'http://localhost:5027/'+drive.cic" alt="">
                </td>
                <td>
                    <img width="300px" :src="'http://localhost:5027/'+drive.license" alt="">
                </td>
                <td>
                    <button class="" @click="SubmitDriver(true)">Xác nhận</button>
                    <button class="" @click="SubmitDriver(false)">Không</button>
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
            drives: [],
            drive: null,
        }
    },
    methods: {
        async SubmitDriver(bien) {
            try {
                // Tạo formData để truyền dữ liệu
                const formData = new FormData();
                formData.append("userId", this.drive.id);
                formData.append("isAccept", bien);

                // Lấy token từ sessionStorage
                const token = sessionStorage.getItem("authToken");
                const response = await axios.post(
                    `http://localhost:5027/api/ManageUser/ExamineDriverSubmit`,
                    formData, // Form data là tham số thứ hai
                    {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        }
                    }
                );
                console.log("Response Submit: ",response);
                if(response.status == 200){
                    this.$router.push({ name: 'employee-driver' });
                }
            } catch (error) {
                console.error("Lỗi khi gọi API:", error);
            }
        },

        async getAllDiver() {
            const id = this.$route.params.id;
            console.log(id);
            const token = sessionStorage.getItem("authToken");
            const response = await axios.get('http://localhost:5027/api/ManageUser/GetAllDriverSubmit', {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            if (response.status == 200) {
                this.drives = response.data.data;
                this.drives.forEach(p => {
                    if (p.id == id) {
                        this.drive = p;
                        console.log(p);
                    }
                })
            }


        }
    },

    async created() {
        await this.getAllDiver();
    },
}
</script>

<style></style>