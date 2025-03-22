<template>

    <div v-if="detail == false">
        <h1>Tất cả đơn thuê xe</h1>
        <button class="btn nutshort" @click="$router.push({ name: 'chart-invoice' });">Thống kê</button>
        <div v-for="item in booking" :key="item.id">
            <div
                style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);margin:10px;padding:30px;margin-bottom:20px;background-color:white;border-radius:5px;">

                <div class="tren">
                    <div class="trai">Tên khách hàng : {{ item.user.name }}</div>
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
                        <p>Thành tiền : {{ formatPricev2(item.total) }}đ</p>
                    </div>
                </div>
                <hr />
                <div class="duoi">
                    <div class="trai">
                        <a @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });"
                            class="btn nutshort">Xem chi tiết</a>
                    </div>
                    <div class="phai">
                        <div>
                            Đã đặt cọc : {{ formatPricev2(item.prePayment) }}đ
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
            id: 0,
        }
    },
    methods: {
        async GetAllBooking() {
            const response = await DriverBookingService.GetAllBookingByUser();
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
        formatPricev2(value) {
            return new Intl.NumberFormat('vi-VN').format(value);
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