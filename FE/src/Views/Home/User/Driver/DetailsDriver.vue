<template>
    <div>

        <h1>Chi tiết đơn chạy của tài xế</h1>

        <!-- <div v-if="booking != null"
            style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);margin:10px;padding:30px;margin-bottom:20px;background-color:white;border-radius:5px;">

            <div class="tren">
                <div class="trai">Tên chủ xe : </div>
                <div class="phai" style="color:orange;font-size:20px;">vsdvds</div>
            </div>
            <hr />
            <div class="giua">
                <div class="trai">
                    <p style="padding:10px 20px;font-size:30px;font-weight:bold;">sdvdsv</p>
                    <img :src="`https://localhost:7265/${booking.post.image}`"
                        style="height:350px;box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);border-radius:5px;" />
                </div>
                <div class="phai" style="width:40%;">
                    <table>
                        <tbody>
                            <tr>
                                <td>Giá xe</td>
                                <td>{{ booking.post.pricePerDay }}/giờ : {{ booking.post.pricePerDay }}/ngày</td>
                            </tr>
                            <tr>
                                <td>Ngày nhận xe</td>
                                <td>{{ booking.recieveOn }}</td>
                            </tr>
                            <tr>
                                <td>Ngày trả xe</td>
                                <td>{{ booking.returnOn }}</td>
                            </tr>
                            <tr>
                                <td>Tổng tiền thuê</td>
                                <td>{{ booking.total }}</td>
                            </tr>
                            <tr>
                                <td>Giảm giá</td>

                                <td v-if="booking.promotion != null">{{ booking.promotion.content }}</td>
                                <td v-else>Không có khuyến mãi</td>

                            </tr>
                            <tr>
                                <td>Thành tiền</td>
                                <td> {{ booking.finalValue }} </td>
                            </tr>
                            <tr>
                                <td>Đã đặt cọc</td>
                                <td>{{ booking.prePayment }}</td>
                            </tr>
                        </tbody>
                    </table>








                </div>
            </div>
            <hr />
            <div class="duoi">
                <div class="trai"> -->
                    <button @click="SubmitHuyChuyen()">Hủy chuyến</button> <!-- v-if="booking.status == 'Accept Booking'" -->
<!-- 
                    <a asp-controller="Posts" asp-action="Details" asp-route-id="@post.Id"
                        style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">

                        <label v-if="booking.status.toString() == 'Hoàn thành'">Thuê xe lại</label>

                        <label v-else>Xem bài viết</label>

                    </a>
                </div>
                <div class="phai">
                    <div>

                        Số tiền cần thanh toán : {{ booking.finalValue - booking.prePayment }}
                    </div>
                </div>
            </div>
        </div>
-->

    </div> 
</template>

<script>
import axios from 'axios';
import BookingVM from '../../../../Model/BookingVM';
import DriverBookingService from '../../../../Service/api/DriverBookingService';
import DriverBookingVM from '../../../../Model/DriverBookingVM';

export default {
    props: {
        id: {
            type: Number,
            default: 0,
            required: true
        },
    },
    data() {
        return {
            bookings: [],
            booking: new DriverBookingVM(),
        }
    },
    methods: {
        async getBooking() {
            const id = this.$route.params.id;
            const response = await DriverBookingService.GetAllBookingByDriver();
            console.log("Detail: ", response);
            if (response.success) {
                this.bookings = response.data;
                if (this.bookings.length > 0) {
                    this.bookings.forEach(p => {
                        if (this.bookings.id == id) {
                            this.booking = p;
                            console.log(p);
                }
                    })
                }

            }
        },
        async SubmitHuyChuyen(){
            const response = await DriverBookingService.HuyChuyen(this.id);
            console.log(response);
        }
    },
    async created() {
        await this.getBooking();
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

table {
    width: 100%;
    border-collapse: collapse;
}

th,
td {
    border: 1px solid rgba(0, 0, 0, 0.1);
    padding: 20px;
    text-align: right;
}

th:first-child,
td:first-child {
    border-left: none;
}

th:last-child,
td:last-child {
    border-right: none;
}

tr:first-child th,
tr:first-child td {
    border-top: none;
}

tr:last-child th,
tr:last-child td {
    border-bottom: none;
}
</style>
