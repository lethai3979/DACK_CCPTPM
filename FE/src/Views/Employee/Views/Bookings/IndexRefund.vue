<template>

    <h1>Danh sách trả cọc</h1>

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
                    <a asp-action="Details" asp-route-id="@item.PostId">@item.Post.Name</a>
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
                    <!-- <a @click="SubmitRequest(item.id)">Trả cọc</a> -->
                </td>
            </tr>
            
        </tbody>
        <tbody v-else>
            Danh sách trống
        </tbody>
    </table>



</template>

<script>
import BookingRequestService from '../../../../Service/api/BookingRequestService';

export default {
    data() {
        return {
            InvoiceRefunds: []
        }
    },
    methods: {
        async getAllRequest() {
            const response = await BookingRequestService.getAllRefundInvoice();
            console.log("REsponse by indexRefund: ", response);
            if (response.success) {
                this.InvoiceRefunds = response.data;
            }
        },
    },
    async created() {
        await this.getAllRequest();
    },


}
</script>

<style></style>
