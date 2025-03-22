<template>
    <div class="Login" v-if="loading == true">
        <div
            style="width: 100%; position: absolute;  z-index: 9999; display: flex; justify-content: center; align-items: center;border-radius: 15px;">
            <div class="spinner-border">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>
    </div>
    <div class="khung">
        <div>
            <span class="icon-closeForm">
                <a class="custom-link" @click="Close()">
                    <i class="ri-close-line"></i>
                </a>
            </span>
            <h3 style="display: flex;justify-content: center;">Xe cần tài xế gần bạn</h3>
            <div class="map-container">
                <div ref="map" style="width: 800px; height: 600px;z-index: 99999999;"></div>
                <!-- <div class="map-controls" v-if="selectedLocation">
                    <div class="location-info">
                        <p>Vĩ độ: {{ selectedLocation.lat.toFixed(6) }}</p>
                        <p>Kinh độ: {{ selectedLocation.lng.toFixed(6) }}</p>
                        <p v-if="selectedLocation.address">Địa chỉ: {{ selectedLocation.address }}</p>
                    </div>
                </div> -->
            </div>
        </div>
    </div>
</template>

<script>
import DriverBookingService from '../../Service/api/DriverBookingService';
import { parse } from 'vue/compiler-sfc';
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
    async created() {
        console.log("Vị trí đc gửi từ ngoài vào: ", this.initialLatLng);
        await this.getBooking(this.initialLatLng.lat, this.initialLatLng.lng);


        // if (this.booking && this.booking.length > 0) {
        //     this.predefinedLocations = this.booking
        //         .filter(booking => booking.latitude && booking.longitude)
        //         .map(booking => ({
        //             id: booking.id,
        //             lat: parseFloat(booking.latitude),
        //             lng: parseFloat(booking.longitude),
        //             recieveOn: booking.recieveOn,
        //             returnOn: booking.returnOn,
        //             image: booking.post.image,
        //             name: booking.post.name
        //         }));
        //     console.log("Các vị trí được trích xuất:", this.predefinedLocations);
        // }
    },
    props: {
        booking: { type: Array, default: () => [] },
        initial: { type: Object, default: () => ({ lat: 10.762622, lng: 106.660172 }) },
    },
    name: "GoogleMap",
    data() {
        return {
            map: null,
            markers: [],
            usermarkers: [],
            selectedLocation: null,
            predefinedLocations: [],
            circle: null,
            initialLatLng: this.initial,
            update: false,
            loading: false
        };
    },
    mounted() {
        const checkGoogleMaps = setInterval(() => {
            if (window.google && window.google.maps) {
                clearInterval(checkGoogleMaps);
                this.initMap();
            }
        }, 100);
    },
    methods: {
        Close() {
            this.loading = false;
            this.$emit('Close');
        },
        // async getBooking(lat, lng) {
        //     const response = await DriverBookingService.GetAllBookingDriver(lat, lng);
        //     if (this.booking && this.booking.length > 0) {
        //         this.predefinedLocations = this.booking
        //             .filter(booking => booking.latitude && booking.longitude)
        //             .map(booking => ({
        //                 id: booking.id,
        //                 lat: parseFloat(booking.latitude),
        //                 lng: parseFloat(booking.longitude),
        //                 recieveOn: booking.recieveOn,
        //                 returnOn: booking.returnOn,
        //                 image: booking.post.image,
        //                 name: booking.post.name
        //             }));
        //         console.log("Các vị trí được trích xuất:", this.predefinedLocations);
        //     }
        // },
        // initMap() {
        //     // Tạo bản đồ với vị trí ban đầu
        //     this.map = new google.maps.Map(this.$refs.map, {
        //         center: this.initialLatLng,
        //         zoom: 13,
        //     });

        //     // Tạo marker vàng cho vị trí ban đầu
        //     this.createMarker1(this.initialLatLng, true);

        //     // Vẽ vòng tròn nét đứt xung quanh marker ban đầu
        //     this.createCircle(this.initialLatLng, 5000); // 10km bán kính

        //     // Tạo các marker đỏ từ mảng predefinedLocations
        //     if (this.predefinedLocations.length > 0) {
        //         this.predefinedLocations.forEach((location) => {
        //             let loca = { lat: location.lat, lng: location.lng };
        //             this.createMarker(loca, false, location);
        //         });
        //     }

        // },


        async getBooking(lat, lng) {
            try {
                this.RemoveMarker();
                const response = await DriverBookingService.GetAllBookingDriverV2(lat, lng);
                this.loading = false;
                // Reset predefinedLocations
                this.predefinedLocations = [];
                if (response.data && response.data.length > 0) {
                    this.predefinedLocations = response.data
                        .filter(booking => booking.latitude && booking.longitude)
                        .map(booking => ({
                            id: booking.id,
                            lat: parseFloat(booking.latitude),
                            lng: parseFloat(booking.longitude),
                            recieveOn: booking.recieveOn,
                            returnOn: booking.returnOn,
                            image: booking.post.image,
                            name: booking.post.name
                        }));

                    console.log("Các vị trí được trích xuất:", this.predefinedLocations);

                    // Tạo các marker đỏ từ mảng predefinedLocations
                    this.initMap();
                } else {
                    console.log("Không tìm thấy booking nào tại vị trí này");
                }
            } catch (error) {
                console.error("Lỗi khi lấy booking:", error);
            }
        },
        initMap() {
            this.RemoveMarker();
            // Tạo bản đồ với vị trí ban đầu
                this.map = new google.maps.Map(this.$refs.map, {
                center: this.initialLatLng,
                zoom: 13,
            });
        
            // Tạo marker vàng cho vị trí ban đầu
            this.createMarker1(this.initialLatLng, true);

            // Vẽ vòng tròn nét đứt xung quanh marker ban đầu
            this.createCircle(this.initialLatLng, 5000); // 10km bán kính

            // Tạo các marker đỏ từ mảng predefinedLocations
            if (this.predefinedLocations.length > 0) {
                this.predefinedLocations.forEach((location) => {
                    let loca = { lat: location.lat, lng: location.lng };
                    this.createMarker(loca, false, location);
                });
            }

            // Thêm sự kiện click trên map
            this.map.addListener('click', async (event) => {
                // Lấy vị trí click
                const clickedLocation = {
                    lat: event.latLng.lat(),
                    lng: event.latLng.lng()
                };
                this.loading = true;
                console.log("Vị trí click: ", clickedLocation.lat, clickedLocation.lng);
                // Cập nhật initialLatLng
                

                this.initialLatLng = clickedLocation;
                this.initMap();
                // this.RemoveMarker();

                // Xóa vòng tròn hiện tại
                if (this.circle) {
                    this.circle.setMap(null);
                }

                // Gọi hàm getBooking với vị trí mới
                await this.getBooking(clickedLocation.lat, clickedLocation.lng);

                // Tạo lại marker và vòng tròn cho vị trí mới
                this.createMarker1(clickedLocation, true);
                this.createCircle(clickedLocation, 7500);
            });
        },
        createMarker1(location, isInitialLocation) {
            const iconPath = "/src/assets/logoWeb/userLocation.png";
            const marker = new google.maps.Marker({
                position: location,
                map: this.map,
                icon: {
                    url: iconPath,
                    scaledSize: new google.maps.Size(40, 40),
                },
                draggable: false,
            });


            // // Lưu lại marker vào mảng
            this.usermarkers.push(marker);
        },

        createMarker(location, isInitialLocation, booking) {
            // Validate location coordinates
            if (!location || typeof location.lat !== 'number' || typeof location.lng !== 'number') {
                console.error('Invalid location coordinates:', location);
                return; // Exit the method if coordinates are invalid
            }

            const iconPath = isInitialLocation
                ? "/src/assets/logoWeb/userLocation.png"
                : "/src/assets/logoWeb/iconLocation.png";


            const marker = new google.maps.Marker({
                position: location,
                map: this.map,
                icon: {
                    url: iconPath,
                    scaledSize: new google.maps.Size(40, 40),
                },
                draggable: false,
            });

            const infoWindow = new google.maps.InfoWindow({
                content: `
                    <div style="padding: 5px; font-size: 14px;">
                        <div v-for="item in booking" :key="item.id" class="box postcar">
                <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                    <img src="http://localhost:5027/${booking.image}" :alt="item.id"
                        style="object-fit: cover; width: 100%; height: 100%;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);" />
                </div>
                <router-link style="font-weight: bold;font-size: 20px; margin-left: 10px;"
                    :to="{ name: 'driver-detail', params: { id: ${booking.id} } }">
                    ${booking.name}
                </router-link>
                <div class="pte1">
                    <div class="giagiam">Bắt đầu ${this.formatDatetime(booking.recieveOn)}</div>
                    <div class="giagiam">Kết thúc ${this.formatDatetime(booking.returnOn)}</div>
                </div>
                <button class="btntest" onclick="RoutesView(${booking.id})">
                        Xem chi tiết
                    </button>
            </div>
                    </div>
                `
            });
            window.RoutesView = (id) => {
                this.$router.push({
                    name: 'driver-detail', params: { id: id }
                });
                this.Close();
            },

                // Lắng nghe sự kiện click vào marker
                marker.addListener("click", () => {
                    this.selectedLocation = location;
                    this.map.panTo(location);
                    this.map.setZoom(16.5);
                    // Hiển thị InfoWindow tại marker
                    infoWindow.open(this.map, marker);
                });

            // Lưu lại marker vào mảng
            this.markers.push(marker);
        },
        createCircle(center, radius) {
            // Xóa vòng tròn cũ nếu đã tồn tại
            if (this.circle) {
                this.circle.setMap(null);
            }

            // Tạo vòng tròn mới
            this.circle = new google.maps.Circle({
                strokeColor: "#FF0000",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: "#FF0000",
                fillOpacity: 0.1,
                map: this.map,
                center: center,
                radius: radius,
                strokeDashArray: [10, 5],
            });

            // Pan map tới vị trí vòng tròn
            // this.map.panTo(center);
        },
        RemoveMarker() {
            // Xóa tất cả các marker hiện tại
            this.markers.forEach(marker => marker.setMap(null));
            this.usermarkers.forEach(marker => marker.setMap(null));
            this.markers = [];
            this.usermarkers = [];
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
};
</script>

<style>
/* map */

.nutthoat {
    position: absolute;
    right: 29%;
}

.khung {
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
    padding: 20px;
    border-radius: 15px;
    position: relative;
}

.nutsubmit {
    position: absolute;
    left: 0;
    margin-left: 30px;
    margin-top: -45px;
    z-index: 1000;
}

.imgPlane {
    width: 20px !important;
    height: 20px !important;
}

.i {
    opacity: 0.75;
    font-size: 14.5px;
}

.btntest {
    margin-inline: 30%;
    margin-block: 3%;
    padding: 10px 15px;
    color: white;
    border: none;
    background-color: var(--color-green2);
    border-radius: 10px;
}
</style>










































<!-- <template>
    <div class="map-container">
      <div ref="map" style="width: 100%; height: 500px;"></div>
      <div class="map-controls" v-if="selectedLocation">
        <div class="location-info">
          <p>Vĩ độ: {{ selectedLocation.lat.toFixed(6) }}</p>
          <p>Kinh độ: {{ selectedLocation.lng.toFixed(6) }}</p>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'GoogleMap',
    data() {
      return {
        map: null,
        markers: [],
        selectedLocation: null,
        predefinedLocations: [
          { lat: 10.772622, lng: 106.670172 },
          { lat: 10.742622, lng: 106.650172 },
          { lat: 10.762622, lng: 106.640172 },
          { lat: 10.772622, lng: 106.640172 },
          { lat: 10.782622, lng: 106.640172 },
          { lat: 10.752622, lng: 106.640172 },
        ],
        initialLatLng: { lat: 10.762622, lng: 106.660172 },
      };
    },
    mounted() {
      const checkGoogleMaps = setInterval(() => {
        if (window.google && window.google.maps) {
          clearInterval(checkGoogleMaps);
          this.initMap();
        }
      }, 100);
    },
    methods: {
      initMap() {
        // Tạo bản đồ với vị trí ban đầu
        this.map = new google.maps.Map(this.$refs.map, {
          center: this.initialLatLng,
          zoom: 12,
        });
  
        // Tạo marker vàng cho vị trí ban đầu
        this.createMarker(this.initialLatLng, true);

  
        // Tạo các marker đỏ từ mảng predefinedLocations
        this.predefinedLocations.forEach((location) => {
          this.createMarker(location, false);
        });
      },
  
      createMarker(location, isInitialLocation) {
        // Chọn icon dựa trên loại marker
        const iconPath = isInitialLocation
          ? '/src/assets/logoWeb/userLocation.png'
          : '/src/assets/logoWeb/iconLocation.png';
        console.log(iconPath);
        // /src/assets/logoWeb/userLocation.png
  
        // Tạo marker với icon tương ứng
        const marker = new google.maps.Marker({
          position: location,
          map: this.map,
          icon: {
            url: iconPath, // Đường dẫn đến icon
            scaledSize: new google.maps.Size(40, 40), // Thay đổi kích thước icon (nếu cần)
          },
          draggable: false,
        });
  
        // Lắng nghe sự kiện click vào marker để hiển thị thông tin vĩ độ, kinh độ
        marker.addListener('click', () => {
          this.selectedLocation = location;
          this.map.panTo(location);
          this.map.setZoom(16.5);
        });
  
        // Lưu lại marker vào mảng
        this.markers.push(marker);
      },
    },
  };
  </script>
   -->