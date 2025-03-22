<template>
    <div class="Login" v-if="loading == true">
        <div
            style="width: 100%; position: absolute;  z-index: 9999; display: flex; justify-content: center; align-items: center;border-radius: 15px;">
            <div class="spinner-border">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>
    </div>

    <div>
        <div v-if="showMap == true" class="Login">
            <MapV3 :show="showMap" :booking="booking" :initial="location2" @Close="Close" @Submit="Submit" />
            <!-- <Map :bien="true" :show="showMap" @Close="Close" @Submit="Submit" /> -->
        </div>
    </div>
    <!-- location != null && -->
    <div v-if="showMap == false">
        <div class="header_list">
            <h3>Danh sách booking cần tài xế</h3>
            <div class="header_list_child" @click="showMap = true;console.log(location)" style="max-width: 450px;">
                <label style="display: flex;">
                    <i>Vị trí của bạn:</i>
                    <i class="add">
                        {{ location }}</i></label>
                <button><img width="24" height="24" src="https://img.icons8.com/fluency-systems-filled/50/edit.png"
                        alt="edit" /></button>
            </div>
        </div>

        <div v-if="booking.length > 0" class="containerPost">
            <div v-for="item in booking" :key="item.id" class="box postcar">
                <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                    <img :src="'http://localhost:5027/' + item.post.image" :alt="item.id"
                        style="object-fit: cover; width: 100%; height: 100%;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);" />
                </div>


                <router-link style="font-weight: bold;font-size: 20px;margin-left: 10px;"
                    :to="{ name: 'driver-detail', params: { id: item.id } }">
                    Lái xe: {{ item.post.name }}
                </router-link>
                <div class="pte1">
                    <div class="giagiam">Bắt đầu {{ formatDatetime(item.recieveOn) }}</div>
                    <div class="giagiam">Kết thúc {{ formatDatetime(item.returnOn) }}</div>
                    <hr>
                    <div class="giagiam">Nhận xe tại: Sài Gòn</div>
                </div>
                <!-- <label for="">Bắt đầu {{ formatDatetime(item.recieveOn) }}</label>
            <label for="">Kết thúc {{ formatDatetime(item.returnOn) }}</label>
            <label for="">Nhận xe tại: Sài Gòn</label> -->

            </div>
        </div>
        <h3 style="margin-inline: 30%;margin-top: 10%;font-size: 34px;color: #434343;" v-else>Danh sách xe yêu cầu trống
        </h3>
    </div>



</template>

<script>
import BookingVM from '../../Model/BookingVM';
import BookingService from '../../Service/api/BookingService';
import DriverBookingService from '../../Service/api/DriverBookingService';
import axios from 'axios';
import Map from '../Home/Map.vue';
import MapService from '../../Service/api/MapService';
import MapV2 from '../Home/MapV2.vue';
import MapV3 from '../Home/MapV3.vue';
export default {
    watch: {
        loading(newVal) {
            // Thay đổi class của body khi login thay đổi
            if (newVal == true) {
                document.body.classList.add("no-scroll");
            } else {
                document.body.classList.remove("no-scroll");
            }
        },
    },
    data() {
        return {
            booking: [],
            lat: null,
            long: null,
            location: null,
            showMap: false,
            loading: false,
            location2: JSON.parse(sessionStorage.getItem('Location')),
        }
    },
    components: {

        Map, MapV2, MapV3
    },
    methods: {
        async Submit(lat, lng) {
            const response = await MapService.getMap(lat, lng);
            this.location = response;
            const res = await DriverBookingService.UpdateLocation(lat, lng);
            await this.getAllBoookingDriver(lat, lng);
            console.log("Res của UpdateLocation: ", res);
            this.showMap = false;
            // console.log(response);
        },

        async getAllBoookingDriver(lat, long) {
            const response = await DriverBookingService.GetAllBookingDriverV1(lat, long);
            console.log("Get all : ", response);
            if (response.success) {
                this.booking = response.data;
                // console.log("This Booking: ", this.booking);
            }
        },

        // async getUserLocation() {
        //     // Kiểm tra xem trình duyệt có hỗ trợ Geolocation API không
        //     if (!navigator.geolocation) {
        //         this.error = "Geolocation is not supported by your browser.";
        //         return;
        //     }

        //     // Yêu cầu vị trí từ người dùng
        //     navigator.geolocation.getCurrentPosition(
        //         async (position) => {
        //             const { latitude, longitude } = position.coords;
        //             this.lat = latitude;
        //             this.long = longitude;
        //             const res = await DriverBookingService.UpdateLocation(latitude, longitude);
        //             await this.getAllBoookingDriver(this.lat, this.long);
        //             this.readMap(latitude, longitude);
        //             this.loading = false;
        //             this.error = null;
        //         },
        //         (err) => {
        //             // Xử lý lỗi nếu người dùng từ chối cấp quyền
        //             switch (err.code) {
        //                 case err.PERMISSION_DENIED:
        //                     this.error = "Permission denied. Please enable location access.";
        //                     break;
        //                 case err.POSITION_UNAVAILABLE:
        //                     this.error = "Position unavailable.";
        //                     break;
        //                 case err.TIMEOUT:
        //                     this.error = "Request timed out.";
        //                     break;
        //                 default:
        //                     this.error = "An unknown error occurred.";
        //             }
        //         }
        //     );
        // },
        readMap(lat, lng) {
            const geocoder = new google.maps.Geocoder();
            // Sử dụng geocoder để lấy địa chỉ từ lat/lng
            geocoder.geocode({ location: { lat: lat, lng: lng } }, (results, status) => {
                if (status === google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        const address = results[0].formatted_address;
                        this.location = address;
                        this.loading = false;
                    } else {
                        console.error('Không tìm thấy địa chỉ nào tại vị trí này.');
                    }
                } else {
                    console.error(`Geocoding thất bại: ${status}`);
                }
            });
        },
        Close() {
            this.showMap = false;
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
        this.loading = true;
        await this.getAllBoookingDriver(this.location2.lat, this.location2.lng);
        this.readMap(this.location2.lat, this.location2.lng);
    },

}
</script>

<style>
.pte1 {
    padding-top: 5px;
    /* display: flex; */
    color: #434343;
    font-size: .9rem;
    font-weight: 500;
    line-height: 22px;
    margin-left: 15px;
}

.add {
    opacity: 0.75;
    display: inline-block;
    max-width: 290px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin-left: 5px;
}
</style>