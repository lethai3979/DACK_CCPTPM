<template>
    <div class="boooking">
        <span class="icon-closeForm2">
            <a class="custom-link" @click="this.$router.push({ name: 'driver-list' });">
                <i class="ri-close-line"></i>
            </a>
        </span>
        <!-- <h5>Chi tiết booking</h5> -->
        <!-- Tài xế -->
        <!-- <div v-if="role == 'Tài xế'">
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
                    </div> -->
        <div class="book" v-if="booking">
            <img class="img" :src="'http://localhost:5027/' + booking.post.image" alt="">
            <div class="tablechild">
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
                <div class="user" v-if="cusrentpage == false">
                    <div>Khách hàng</div>
                    <img class="imgUser" :src="'http://localhost:5027/' + booking.user.image" :alt="booking.id">
                    <div class="hover">{{ booking.user.name }}</div>
                </div>
                <div class="user" v-if="cusrentpage == true">
                    <div>Chủ xe</div>
                    <img class="imgUser" :src="'http://localhost:5027/' + booking.post.user.image" :alt="booking.post.user.id">
                    <div class="hover">{{ booking.post.user.name }}</div>
                </div>
                <table class="tableAll">
                    <thead class="tableBooking">
                        <tr>
                            <th>Lái xe: </th>
                            <th>{{ booking.post.name }}</th>
                        </tr>
                        <tr>
                            <th>Loại hộp số: </th>
                            <th v-if="booking.post.gear">Số tự động</th>
                            <th v-else>Số sàn</th>
                        </tr>
                        <tr>
                            <th>Số chổ ngồi: </th>
                            <th>{{ booking.post.seat }} chổ</th>
                        </tr>
                        <tr>
                            <th>Địa chỉ nhận xe: </th>
                            <th>{{ booking.post.rentLocation }}</th>
                        </tr>
                        <tr>
                            <th>Thời gian nhận xe: </th>
                            <th>{{ formatDatetime(booking.recieveOn) }}</th>
                        </tr>
                        <tr>
                            <th>Thời gian trả xe: </th>
                            <th>{{ formatDatetime(booking.returnOn) }}</th>
                        </tr>
                        <tr>
                            <th>Vị trí nhận xe: </th>
                            <th>{{ readMap(booking.latitude, booking.longitude) }}</th>
                        </tr>
                    </thead>
                </table>
                <div class="doublebutton">
                    <router-link class="btn short w200"
                        :to="{ name: 'user-post-detail', params: { id: booking.post.id } }">
                        <label style="margin-top: 5px;">Xem chi tiết xe</label>
                    </router-link>
                    <button @click="Submit()" class="btn short w200">Xác nhận đơn đặt</button>
                </div>
            </div>

        </div>

    </div>


</template>

<script>
import BookingVM from '../../Model/BookingVM';
import BookingService from '../../Service/api/BookingService';
import DriverBookingService from '../../Service/api/DriverBookingService';

export default {
    data() {
        return {
            booking: new BookingVM(),
            cusrentpage: false
        }
    },
    methods: {
        async Submit() {
            const id = this.$route.params.id;
            const response = await DriverBookingService.AddBookingDriver(id);
            console.log(response);
            if (response.data.success) {
                console.log("Xác nhận thành công");
                this.$router.push({ name: 'driver-list' });
            }
        },
        async getAllBoookingDriver() {
            const id = this.$route.params.id;
            const response = await BookingService.GetBookingById(id);
            if (response.success) {
                this.booking = response.data;
            }

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
        readMap(lat, lng) {
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
    created() {
        this.getAllBoookingDriver();
    },

}
</script>

<style>
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

.doublebutton {
    margin-top: 20px;
    margin-inline: 10%;
    display: flex;
    gap: 20px;
    width: 100%;
}

.boooking {
    position: relative;
    padding: 20px 30px;
    width: 80%;
    margin-inline: 110px;
    background-color: var(--bg-colorWhite);
    border-radius: 15px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
}

.book {
    display: flex;

}

.imgUser {
    width: 150px !important;
    height: 150px !important;
    border: 1px solid;
    object-fit: cover;
    border-radius: 100%;
}

.book .img {
    width: 40%;
    height: 500px;
    object-fit: cover;
    /* border: 1px solid; */
    margin-right: 50px;
    border-radius: 15px;
}

.tablechild {
    width: 60%;
}

.user {
    margin-inline: 20px;
    padding: 10px;
    width: 90% !important;
    border: 1px solid #ccc;
    border-radius: 10px;
    justify-items: center;
    justify-content: center;
    align-items: center;
    align-content: center;
    margin-bottom: 20px;
}

.tableAll {
    margin-inline: 20px;
    width: 90% !important;
    /* border: 1px solid; */
}

.tableBooking tr {
    display: flex;
    justify-content: space-between;
    margin-block: 7px;
}
</style>