<template>
    <h1>Lịch sử xe đã nhận</h1>

    <div v-for="item in booking" :key="item.id">


        <!-- var currentUser = await UserManager.FindByIdAsync(item.Post.UserId!); -->
        <div
            style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);margin:10px;padding:30px;margin-bottom:20px;background-color:white;border-radius:5px;">

            <div class="tren">
                <div class="trai">Tên chủ xe : {{ item.post.name }}</div>
                <div class="phai" style="color:orange;font-size:20px;">{{ item.status }}</div>
            </div>
            <hr />
            <div class="giua">
                <div class="trai" style="display:flex;">
                    <img :src="'https://localhost:7265/' + item.post.image"
                        style="height:150px;box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);border-radius:5px;" />
                    <span style="padding:10px 20px;font-size:23px;font-weight:bold;">{{ item.post.name }}</span>
                </div>
                <div class="phai">
                    <p>Số tiền gốc : {{ item.post.price }}/giờ</p>
                    <p>Thành tiền : {{ item.total }}</p>
                </div>
            </div>
            <hr />
            <div class="duoi">
                <div class="trai">
                    <a v-if="item.status == 'Accept Booking'" @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });"
                        style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Thanh toán</a>


                    <a @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });"
                        style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Xem chi tiết</a>

                    <a v-if="item.recieveOn <= Date.now && item.status == 'Waiting'" @click="console.log(1 + item.id);"
                        style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Yêu cầu trả cọc 1</a>

                    <a v-if="item.recieveOn >= Date.now || item.status == 'Canceled'" aria-disabled="true"
                        style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Không thể trả cọc</a>

                    <!-- <a v-if="item.recieveOn <= Date.now" asp-action="Delete" hidden asp-route-id="@item.Id" style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Yêu cầu trả cọc</a>
                               
                                <a v-else asp-action="Delete" asp-route-id="@item.Id" style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Yêu cầu trả cọc</a> -->

                </div>
                <div class="phai">
                    <div>
                        Đã đặt cọc : {{ item.prePayment }}
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>

<script>
import BookingService from '../../../Service/api/BookingService';
import DriverBookingService from '../../../Service/api/DriverBookingService';

export default {
    data() {
        return {
            booking: null
        }
    },
    methods: {
        async GetAllBooking() {
            const response = await DriverBookingService.GetAllDriverBooking();
            console.log("Booking:", response);
            this.booking = response.data;
        },
    },

    async created() {
        await this.GetAllBooking();
    },

}
</script>

<style>
.tren,
.giua,
.duoi {
    display: flex;
    justify-content: space-between;
    align-items: center;

}
</style>

    