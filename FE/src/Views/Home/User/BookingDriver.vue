<template>
    <div v-if="detail == false">
        <h1>Tất cả đơn chạy xe</h1>

        <div v-for="item in booking" :key="item.id">

            <!-- var currentUser = await UserManager.FindByIdAsync(item.Post.UserId!); -->
            <div
                style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);margin:10px;padding:30px;margin-bottom:20px;background-color:white;border-radius:5px;">

                <div class="tren">
                    <div class="trai" >Tên chủ xe : {{ item.post.user.name }}</div>
                    <div class="phai" style="color:orange;font-size:20px;">{{ item.status }}</div>
                </div>
                <hr />
                <div class="giua">
                    <div class="trai" style="display:flex;">
                        <img :src="'http://localhost:5027/' + item.post.image"
                            style="height:150px;box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);border-radius:5px;" />
                        <span style="padding:10px 20px;font-size:23px;font-weight:bold;">{{ item.post.name }}</span>
                    </div>
                    <div class="phai">
                        <p>Số tiền gốc : {{ formatPrice(item.post.pricePerHour) }}/giờ</p>
                        <p>Số tiền gốc : {{ formatPrice(item.post.pricePerDay) }}/ngày</p>
                        <p>Thành tiền : {{ item.total }}</p>
                    </div>
                </div>
                <hr />
                <div class="duoi">
                    <div class="trai">
                        <a @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });"
                            class="btn nutshort">Xem chi tiết</a>
                        <a v-if="CheckDay(item.recieveOn) == true" style="margin-left: 20px;" @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });"
                            class="btn nutshort">Hủy đơn chạy xe</a>
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
    </div>

    <DetailsDriver :id="id" v-else @Close="Close" />
</template>


<script>
import BookingVM from '../../../Model/BookingVM';
import DriverBookingVM from '../../../Model/DriverBookingVM';
import BookingService from '../../../Service/api/BookingService';
import DriverBookingService from '../../../Service/api/DriverBookingService';
import DetailsDriver from './Driver/DetailsDriver.vue';
export default {
    components: {
        DetailsDriver
    },
    data() {
        return {
            booking: [new BookingVM()],
            detail: false,
            id: 0
        }
    },
    methods: {
        async GetAllBooking() {
            const response = await DriverBookingService.GetAllBookingByDriver();
            console.log("Booking:", response);

            this.booking = response.data;
        },
        Close() {
            this.detail = false;
        },
        formatPrice(price) {
            if (price >= 1000000) {
                return (price / 1000000).toFixed(1).replace('.0', '') + "Tr";
            } else if (price >= 1000) {
                return (price / 1000).toLocaleString('en').replace(/,/g, '.') + "k";
            } else {
                return price.toString();
            }
        },
        CheckDay(daystring) {
            const day = new Date(daystring);

            // Lấy thời gian hiện tại
            const now = new Date();

            // Tính khoảng cách thời gian (mili giây)
            const diffTime = day - now; // `day` trừ `now`

            // Chuyển thời gian chênh lệch từ mili giây sang ngày
            const diffDays = diffTime / (1000 * 60 * 60 * 24);

            console.log("Khoảng cách ngày:", diffDays);

            // Trả về true nếu chênh lệch ngày lớn hơn 1
            return diffDays > 1;
        }
        
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