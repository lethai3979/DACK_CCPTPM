<template>
    <!-- <h1>Danh sách yêu cầu hủy chuyến</h1> -->

    <table class="table">
        <thead>
            <tr>
                <th>
                    Bài đăng
                </th>
                <th>
                    Ngày nhận
                </th>
                <th>
                    Ngày trả
                </th>
                <th>
                    Tổng tiền
                </th>
                <th>
                    Thành tiền
                </th>
                <th>
                    Đặt cọc
                </th>
                <th>
                    Trạng thái
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody v-if="BookingRequest && BookingRequest.length > 0">
            <tr v-for="item in BookingRequest" :key="item.id">
                <td>
                    <a asp-action="Details" asp-route-id="@item.PostId">{{ item.post.name }}</a>
                </td>
                <td>
                    {{ item.recieveOn }}
                </td>
                <td>
                    {{ item.returnOn }}
                </td>
                <td>
                    {{ item.total }}
                </td>
                <td>
                    {{ item.finalValue }}
                </td>
                <td>
                    {{ item.prePayment }}
                </td>
                <td>
                    {{ item.status }}
                </td>
                <td>
                    <router-link :to="{ name: 'admin-request-detail', params: { id: item.id } }">
                        Chi tiết {{ item.id }}
                    </router-link>
                </td>
            </tr>
            
        </tbody>
        <tbody v-else>
            Danh sách trống
        </tbody>
    </table>



</template>

<script>
import BookingVM from '../../../../Model/BookingVM';
import BookingRequestService from '../../../../Service/api/BookingRequestService';

export default {
    data() {
        return {
            BookingRequest: [
                new BookingVM(),
            ]
        }
    },
    methods: {
        async getAllRequest() {
            const response = await BookingRequestService.getAllCancel();
            console.log("REsponse by index: ", response);
            if (response.success) {
                this.BookingRequest = response.data;
            }
        },
    },
    async created() {
        await this.getAllRequest();
    },


}
</script>
