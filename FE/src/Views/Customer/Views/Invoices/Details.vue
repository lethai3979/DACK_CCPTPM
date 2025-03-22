<template>
    <div v-if="invoice != null">
        <h1 style="margin-inline: 10%;">{{ invoice.booking.post.name }}</h1>
        <div v-if="invoice != null" :class="{ ispay : invoice.booking.status == 'Accept Booking' && invoice.booking.isPay == false }"
            style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);margin:10px;padding:30px;margin-bottom: 20px;background-color:white;border-radius:15px;width: 1000px;margin-inline: 10%;position: relative;">
            <span class="icon-closeForm" style="margin-right: 0px;">
                <a class="custom-link" onclick="javascript:history.go(-1);">
                    <i class="ri-close-line"></i>
                </a>
            </span>
            <div class="tren">
                <div @click="this.$router.push({ name: 'user-infor', params: { userId: invoice.booking.post.user.id } })"
                    class="trai">Tên chủ xe : {{ invoice.booking.post.user.name }}</div>

                <div class="phai" style="color:orange;font-size:20px;margin-right: 20px;"
                    v-if="invoice.refundInvoice == false">{{
                        invoice.booking.status }}</div>
                <div class="phai" style="color:orange;font-size:20px; margin-right: 20px;" v-else>Hóa đơn hoàn tiền
                </div>
            </div>
            <hr />
            <div class="giua">
                <div class="trai" style="display: block;">
                    <img :src="`http://localhost:5027/${invoice.booking.post.image}`"
                        style="height:350px;box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);border-radius:5px;" />
                    <div style="display: flex;gap: 10px;margin-top: 15px;">
                        <div v-for="item in invoice.booking.post.images" :key="item">
                            <img width="148.5px" style="object-fit: cover; border-radius: 5px;"
                                :src="`http://localhost:5027/${item.url}`" alt="">
                        </div>
                    </div>
                </div>
                <div class="phai" style="width:40%;">
                    <div class="user"
                        v-if="invoice != null && invoice.driverBooking != null && invoice.driverBooking.driver.user != null"
                        @click="this.$router.push({ name: 'user-infor', params: { userId: invoice.driverBooking.driver.user.id } })">
                        <img class="imgUser" :src="'http://localhost:5027/' + invoice.driverBooking.driver.user.image"
                            style="width: 150px;" :alt="invoice.driverBooking.driver.user.id">
                        <div>Tài xế: {{ invoice.driverBooking.driver.user.name }}</div>
                        <div>Số điện thoại: {{ invoice.driverBooking.driver.user.phone }}</div>
                        <div>Email: {{ invoice.driverBooking.driver.user.email }}</div>
                    </div>
                    <div class="user" v-else>
                        <h3>Chưa có tài xế</h3>
                    </div>
                    <table>
                        <tbody>
                            <tr>
                                <td>Giá xe</td>
                                <td>{{ formatPrice(invoice.booking.post.pricePerHour) }}/giờ : {{
                                    formatPrice(invoice.booking.post.pricePerDay) }}/ngày</td>
                            </tr>
                            <tr>
                                <td>Ngày nhận xe</td>
                                <td>{{ formatDatetime(invoice.booking.recieveOn) }}</td>
                            </tr>
                            <tr>
                                <td>Ngày trả xe</td>
                                <td>{{ formatDatetime(invoice.booking.returnOn) }}</td>
                            </tr>
                            <tr>
                                <td>Tổng tiền thuê</td>
                                <td>{{ invoice.booking.total }}</td>
                            </tr>
                            <tr>
                                <td>Giảm giá</td>

                                <td v-if="invoice.booking.promotion != null">{{ invoice.booking.promotion.content }}
                                </td>
                                <td v-else>Không có khuyến mãi</td>
                            </tr>
                            <tr>
                                <td>Thành tiền</td>
                                <td> {{ invoice.booking.finalValue }} </td>
                            </tr>
                            <tr>
                                <td>Đã đặt cọc</td>
                                <td>{{ invoice.booking.prePayment }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <hr />
            <div class="duoi">
                <button @click="Thanhtoan()" class="btn nutshort"
                    v-if="invoice.booking.status == 'Accept Booking' && invoice.booking.isPay == false">Thanh
                    toán</button>
            </div>
        </div>
    </div>

</template>

<script>
import InvoiceVM from '../../../../Model/InvoiceVM';
import axios from 'axios';
import InvoiceService from '../../../../Service/api/InvoiceService'
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
            invoices: [new InvoiceVM()],
            invoice: new InvoiceVM(),
        }
    },
    methods: {
        async Thanhtoan() {
            const id = this.$route.params.id;
            var bookingId = id;
            const formData = new FormData();
            formData.append("bookingId", id);
            formData.append("isMobile", true);
            const token = sessionStorage.getItem("authToken");
            const response = await axios.post(`http://localhost:5027/api/User/Invoice/MomoPayment/${bookingId}`,
                formData
                , {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
            if (response.status !== 200) {
                console.log("Lỗi");
            }
            else {
                window.location.href = response.data;
            }
            console.log(response);
        },
        async getInvoice() {
            const id = this.$route.params.id;
            const response = await InvoiceService.getAllInvoicePersonal();
            this.invoices = response.data;
            this.invoices.forEach(p => {
                if (p.id == id) {
                    this.invoice = p;
                    console.log("Invoice đc lấy ra: ", this.invoice);
                }
            });
        },
        formatPrice(value) {
            return new Intl.NumberFormat('vi-VN').format(value);
        },
        formatDatetime(datetimeStr) {
            const date = new Date(datetimeStr);
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0
            const year = date.getFullYear();
            return `${hours}:${minutes} ${day}-${month}-${year}`;
        },
    },
    async created() {
        await this.getInvoice();
    },

}
</script>


<style>
.ispay{border: 0.05px solid red;}
.user {
    /* margin-inline: 20px; */
    padding: 10px;
    width: 100% !important;
    border: 1px solid #ccc;
    border-radius: 10px;
    justify-items: center;
    justify-content: center;
    align-items: center;
    align-content: center;
    margin-bottom: 20px;
}

.row12 {
    display: flex;
    justify-content: space-between;
}

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