<template>
    <div>

        <!-- <h1 style="margin-inline: 10%;" v-if="booking.post.user.id !== user.id">{{ booking.post.name }}</h1> -->

        <div v-if="booking != null"
            style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);margin:10px;padding:30px;margin-bottom: 20px;background-color:white;border-radius:5px;width: 1000px;margin-inline: 10%;position: relative;">
            <span class="icon-closeForm" style="margin-right: 0px;">
                <a class="custom-link" onclick="javascript:history.go(-1);">
                    <i class="ri-close-line"></i>
                </a>
            </span>
            <div class="tren">
                <h1 @click="this.$router.push({ name: 'user-post-detail', params: { id: booking.post.id } })"
                    class="trai" style="margin-bottom: -10px;">{{ booking.post.name }}</h1>
                <div class="phai" style="font-size:20px;margin-right: 20px;">
                    <label v-if="booking.status == 'Pending'" style="color: orange;">
                        Chờ xác nhận
                    </label>
                    <label v-if="booking.status == 'Accept Booking'" style="color: orange;">
                        Vui lòng thanh toán
                    </label>
                    <label v-if="booking.status == 'Waiting'" style="color: orange;">
                        Chờ nhận xe
                    </label>
                    <label v-if="booking.status == 'Renting'" style="color: rgb(0, 102, 254);">
                        Đang thuê
                    </label>
                    <label v-if="booking.status == 'Complete'" style="color: green;">
                        Hoàn thành
                    </label>
                    <label v-if="booking.status == 'Proccessing'" style="color: red;">
                        Đã yêu cầu hủy
                    </label>
                    <label v-if="booking.status == 'Refunded'" style="color: red;">
                        Đã trả cọc 
                    </label>
                    <label v-if="booking.status == 'Canceled'" style="color: red;">
                        Đã hủy chuyến 
                    </label>
                </div>
            </div>
            <hr />
            <div class="giua">
                <div class="trai" style="display: block;">
                    <img :src="`http://localhost:5027/${booking.post.image}`"
                        style="height:350px;box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);border-radius:5px;" />
                    <div style="display: flex;gap: 10px;margin-top: 15px;">
                        <div v-for="item in booking.post.images" :key="item">
                            <img width="148.5px" style="object-fit: cover;border-radius: 5px;"
                                :src="`http://localhost:5027/${item.url}`" alt="">
                        </div>
                    </div>
                    <div class="addressV1">
                        Địa chỉ nhận xe: {{ address }}.
                    </div>
                </div>
                <div class="phai" style="width:40%;">
                    <div v-if="role == 'Tài xế'">
                        <div style="display: flex;gap: 20px;margin-bottom: 15px;">
                            <label :class="{ mo: cusrentpage == true }" @click="cusrentpage = false">
                                <i>Khách hàng</i>
                                <div class="gach"></div>
                            </label>
                            <label :class="{ mo: cusrentpage == false }" @click="cusrentpage = true">
                                <i>Chủ xe</i>
                                <div class="gach"></div>
                            </label>
                        </div>

                        <div class="user" v-if="cusrentpage == false"
                            @click="this.$router.push({ name: 'user-infor', params: { userId: booking.user.id } })">
                            <img class="imgUser" :src="'http://localhost:5027/' + booking.user.image"
                                style="width: 150px;" :alt="booking.user.id">
                            <div>Khách hàng: {{ booking.user.name }}</div>
                            <div>Số điện thoại: {{ booking.user.phoneNumber }}</div>
                            <div>Email: {{ booking.user.email }}</div>
                        </div>
                        <div class="user" v-if="cusrentpage == true"
                            @click="this.$router.push({ name: 'user-infor', params: { userId: booking.post.user.id } })">
                            <img class="imgUser" :src="'http://localhost:5027/' + booking.user.image"
                                style="width: 150px;" :alt="booking.user.id">
                            <div>Chủ xe: {{ booking.post.user.name }}</div>
                            <div>Số điện thoại: {{ booking.post.user.phoneNumber }}</div>
                            <div>Email: {{ booking.post.user.email }}</div>
                        </div>
                    </div>
                    <!-- Khách hàng -->
                    <div v-if="role == 'Khách hàng'">
                        <div style="display: flex;gap: 20px;margin-bottom: 15px;">
                            <label :class="{ mo: cusrentpage == true }" @click="cusrentpage = false">
                                <i>Chủ xe</i>
                                <div class="gach"></div>
                            </label>
                            <label :class="{ mo: cusrentpage == false }" @click="cusrentpage = true">
                                <i>Tài xế</i>
                                <div class="gach"></div>
                            </label>
                        </div>

                        <div class="user" v-if="cusrentpage == false"
                            @click="this.$router.push({ name: 'user-infor', params: { userId: booking.post.user.id } })">
                            <img class="imgUser" :src="'http://localhost:5027/' + booking.post.user.image"
                                style="width: 150px;" :alt="booking.post.user.id">
                            <div>Chủ xe: {{ booking.post.user.name }}</div>
                            <div>Số điện thoại: {{ booking.post.user.phoneNumber }}</div>
                            <div>Email: {{ booking.post.user.email }}</div>
                        </div>
                        <div class="user" v-if="cusrentpage == true"
                            @click="this.$router.push({ name: 'user-infor', params: { userId: booking.driver.user.id } })">
                            <img class="imgUser" :src="'http://localhost:5027/' + booking.user.image"
                                style="width: 150px;" :alt="booking.user.id">
                            <div>Tài xế: {{ booking.driver.user.name }}</div>
                            <div>Số điện thoại: {{ booking.driver.user.phoneNumber }}</div>
                            <div>Email: {{ booking.driver.user.email }}</div>
                        </div>
                    </div>
                    <!-- Chủ xe  -->
                    <div v-if="role == 'Chủ xe'">
                        <div style="display: flex;gap: 20px;margin-bottom: 15px;">
                            <label :class="{ mo: cusrentpage == true }" @click="cusrentpage = false">
                                <i>Khách hàng</i>
                                <div class="gach"></div>
                            </label>
                            <!-- <label :class="{ mo: cusrentpage == false }" @click="cusrentpage = true">
                                <i>Tài xế</i>
                                <div class="gach"></div>
                            </label> -->
                        </div>
                        <div class="user" v-if="cusrentpage == false"
                            @click="this.$router.push({ name: 'user-infor', params: { userId: booking.user.id } })">
                            <img class="imgUser" :src="'http://localhost:5027/' + booking.user.image"
                                style="width: 150px;" :alt="booking.user.id">
                            <div>Khách hàng: {{ booking.user.name }}</div>
                            <div>Số điện thoại: {{ booking.user.phoneNumber }}</div>
                            <div>Email: {{ booking.user.email }}</div>
                        </div>
                        <!-- <div class="user" v-if="cusrentpage == true"
                            @click="this.$router.push({ name: 'user-infor', params: { userId: booking.post.user.id } })">
                            <img class="imgUser" :src="'http://localhost:5027/' + booking.user.image"
                                style="width: 150px;" :alt="booking.user.id">
                            <div>Tài xế: {{ booking.driver.user.name }}</div>
                            <div>Số điện thoại: {{ booking.driver.user.phone }}</div>
                            <div>Email: {{ booking.driver.user.email }}</div>
                        </div> -->

                    </div>
                    <table>
                        <tbody>
                            <tr>
                                <td>Yêu cầu tài xế</td>
                                <td v-if="booking.isRequireDriver">Có</td>
                                <td v-else>Không</td>
                            </tr>
                            <tr>
                                <td>Giá xe</td>
                                <td>{{ formatPrice(booking.post.pricePerHour) }}đ/giờ : {{
                                    formatPrice(booking.post.pricePerDay) }}đ/ngày</td>
                            </tr>
                            <tr>
                                <td>Ngày nhận xe</td>
                                <td>{{ formatDatetime(booking.recieveOn) }}</td>
                            </tr>
                            <tr>
                                <td>Ngày trả xe</td>
                                <td>{{ formatDatetime(booking.returnOn) }}</td>
                            </tr>
                            <tr>
                                <td>Tổng tiền thuê</td>
                                <td>{{ formatPrice(booking.total) }}đ</td>
                            </tr>
                            <tr v-if="booking.post.user.id !== user.id">
                                <td>Giảm giá</td>
                                <td v-if="booking.promotion != null">{{ booking.promotion.content }}</td>
                                <td v-else>Không có khuyến mãi</td>
                            </tr>
                            <tr>
                                <td>Thành tiền</td>
                                <td> {{ formatPrice(booking.finalValue) }}đ </td>
                            </tr>
                            <tr>
                                <td>Đã đặt cọc</td>
                                <td>{{ formatPrice(booking.prePayment) }}đ</td>
                            </tr>
                        </tbody>
                    </table>








                </div>
            </div>
            <hr />
            <div v-if="role == 'Chủ xe'" class="duoi">
                <div class="trai" v-if="booking.status == 'Pending'">
                    <button @click="ConfirmBooking(true, booking.id)" class="btn nutshort">Chấp nhận</button>
                    <button @click="ConfirmBooking(false, booking.id)" style="margin-left: 10px;"
                        class="btn1 nutshort">Từ chối</button>
                </div>
            </div>
            <div v-if="role == 'Tài xế' && CheckDay(booking.recieveOn) == true" class="duoi">
                <div class="trai">
                    <button @click="HuyDon()" class="btn nutshort">Hủy đơn</button>
                </div>
            </div>
            <div class="duoi" v-if="role == 'Khách hàng'">
                <div class="trai">
                    <button @click="Thanhtoan()" class="btn nutshort"
                        v-if="booking.status == 'Accept Booking' && booking.isPay == false">Thanh toán</button>

                    <a v-else @click="this.$router.push({ name: 'user-post-detail', params: { id: booking.post.id } })"
                        class="btn nutshort">

                        <label v-if="booking.status.toString() == 'Hoàn thành'">Thuê xe lại</label>

                        <label v-else>Xem bài viết</label>

                    </a>
                </div>
                <div class="phai">
                    <div>

                        Số tiền cần thanh toán : {{ formatPrice(booking.finalValue - booking.prePayment) }}đ
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import BookingVM from '../../../../Model/BookingVM';
import BookingService from '../../../../Service/api/BookingService';
import UserVM from '../../../../Model/UserVM';
import AuthenticationService from '../../../../Service/api/AuthenticationService';
import DriverBookingService from '../../../../Service/api/DriverBookingService';

export default {
    data() {
        return {
            booking: new BookingVM(),
            user: new UserVM(),
            role: 0,
            cusrentpage: false,
            address: null
        }
    },
    methods: {
        async ConfirmBooking(bien, id) {
            const response = await BookingService.ConfirmBooking(id, bien);
            if (response.success) {
                this.$router.push({ name: 'user-profile-mycar' });
            }
        },
        async HuyDon() {
            const response = await DriverBookingService.CancelDriverBooking(this.booking.id);
            console.log("Response huyr ddown tafi xees: ", response);
            if (response.success) {
                this.$router.push({ name: 'user-profile-driver' });
            }
        },
        async getBooking() {
            const id = this.$route.params.id;
            const response = await BookingService.GetBookingById(id);
            if (response.success) {
                console.log("Detail: ", response);
                this.booking = response.data;
                await this.readMap(parseFloat(this.booking.latitude), parseFloat(this.booking.longitude));
            }
        },
        async Thanhtoan() {
            const id = this.$route.params.id;
            var bookingId = id;
            const formData = new FormData();
            const token = sessionStorage.getItem("authToken");
            const response = await axios.post(`http://localhost:5027/api/User/Invoice/MomoPayment/${bookingId}&&false`, null,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
            // console.log("Response trả về thanh toán: ",response);
            if (response.status !== 200) {
                console.log("Lỗi");
            }
            else {
                // console.log("Thanh cong: ",response);
                window.location.href = response.data;
            }
            console.log(response);
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
        async getUser() {
            const response = await AuthenticationService.getUser();
            this.user = response;
            console.log("response: ", this.user);
            if (this.user.id == this.booking.post.user.id) {
                this.role = 'Chủ xe';
            }
            else if (this.user.id == this.booking.user.id) {
                this.role = 'Khách hàng';
            }
            else {
                this.role = 'Tài xế';
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
        },
        async readMap(lat,lng){
            const geocoder = new google.maps.Geocoder();
                // Sử dụng geocoder để lấy địa chỉ từ lat/lng
                geocoder.geocode({ location: { lat: lat, lng: lng } }, (results, status) => {
                    if (status === google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            const address = results[0].formatted_address;
                            console.log(`Address: ${address}`);
                            this.address = address;

                            // Bạn có thể hiển thị địa chỉ trên giao diện hoặc làm gì đó với nó
                            // alert(`Vị trí bạn chọn: ${address}`);
                        } else {
                            console.error('Không tìm thấy địa chỉ nào tại vị trí này.');
                        }
                    } else {
                        console.error(`Geocoding thất bại: ${status}`);
                    }
                });
        },
    },
    async created() {
        await this.getBooking();
        this.getUser();
    },

}
</script>



<style>
.addressV1 {
    margin-top: 20px;
    max-width: 480px; /* Giới hạn độ rộng tối đa */
    word-wrap: break-word; /* Tự động xuống hàng khi vượt quá giới hạn */
    word-break: break-word; /* Đảm bảo các từ dài sẽ bị cắt và xuống hàng */
    overflow-wrap: break-word; /* Hỗ trợ thêm cho các trình duyệt cũ */
    white-space: normal; /* Cho phép xuống dòng (mặc định) */
}
.gach {
    height: 4px;
    background-color: aqua;
    margin-top: 0px;
}

.mo {
    opacity: 0.5;
    color: black;
}

.mo .gach {
    background-color: #a0a0a0 !important;
}

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












<!-- 

@* 
<div>
    <h4>Booking</h4>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">
            Bài đăng
        </dt>
        <dd class="col-sm-10">
            <div>
                Id:
                @Html.DisplayFor(model => model.Post.Id)
            </div>
            <div>
                Name:
                @Html.DisplayFor(model => model.Post.Name)
            </div>
        </dd>

        <dt class = "col-sm-2">
            Ngày nhận
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.RecieveOn)
        </dd>
        <dt class = "col-sm-2">
            Ngày trả
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.ReturnOn)
        </dd>
        <dt class="col-sm-2">
            Tổng cộng
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.Total)
        </dd>
        <dt class = "col-sm-2">
            Giảm giá
        </dt>
        <dd class = "col-sm-10">
            @Html.DisplayFor(model => model.Promotion.Content)
        </dd>
        <dt class="col-sm-2">
            Thành tiền
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.FinalValue)
        </dd>
        <dt class="col-sm-2">
            Đặt cọc
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.PrePayment)
        </dd>
        <dt class="col-sm-2">
            Đã đặt cọc
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.IsPay)
        </dd>
    </dl>
</div> *@
<div>
    <a href="javascript:history.go(-1);">Quay lại</a>
</div> -->
