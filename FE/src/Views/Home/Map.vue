<script setup>
import { onMounted, ref, watch } from "vue";
import maplibregl from "maplibre-gl";
import "maplibre-gl/dist/maplibre-gl.css"; // Quan trọng: Import CSS của MapLibre

const lat = ref(0);
const lng = ref(0);
const loading = ref(false);
const address = ref(null);
const map = ref(null);
const mapContainer = ref(null);
const marker = ref(null);

onMounted(() => {
    // Khởi tạo bản đồ MapLibre GL JS
    map.value = new maplibregl.Map({
        container: mapContainer.value,
        style: "https://basemaps.cartocdn.com/gl/voyager-gl-style/style.json",
        center: [106.66868578575641, 10.831282278047903],
        zoom: 13
    });

    // Thêm các điều khiển điều hướng
    map.value.addControl(new maplibregl.NavigationControl());

    // Thêm sự kiện click vào bản đồ
    map.value.on("click", (e) => {
        const clickedLat = e.lngLat.lat;
        const clickedLng = e.lngLat.lng;

        // Cập nhật vị trí
        lat.value = clickedLat;
        lng.value = clickedLng;

        // Di chuyển hoặc tạo mới marker tại vị trí được click
        updateMarker(clickedLat, clickedLng);
    });
});

// Hàm di chuyển hoặc tạo mới marker
async function updateMarker(newLat, newLng) {
    // Nếu marker đã tồn tại, hãy loại bỏ nó trước
    if (marker.value) {
        marker.value.remove();
    }
    loading.value = true;
    await UpdateMap(newLat, newLng);
    // Tạo marker mới tại vị trí được click
    marker.value = new maplibregl.Marker({
        color: "#FF0000", // Màu đỏ 
        draggable: true // Cho phép kéo marker
    })
        .setLngLat([newLng, newLat])
        .addTo(map.value);
}

// Lấy vị trí hiện tại của người dùng
function getCurrentLocation() {
    loading.value = true;
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(async (position) => {
            const currentLat = position.coords.latitude;
            const currentLng = position.coords.longitude;

            // Cập nhật vị trí
            lat.value = currentLat;
            lng.value = currentLng;
            await UpdateMap(currentLat, currentLng);
            // Di chuyển hoặc tạo mới marker tại vị trí hiện tại
            updateMarker(currentLat, currentLng);
            // Di chuyển bản đồ đến vị trí hiện tại
            map.value.flyTo({
                center: [currentLng, currentLat],
                zoom: 13
            });
        }, (error) => {
            // Xử lý lỗi định vị
            console.error("Error getting location:", error);
            alert("Unable to retrieve your location");
        });
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}
async function UpdateMap(lat, lng) {
    const response = await MapService.getMap(lat, lng);
    console.log(response.display_name);
    address.value = response.display_name;
    loading.value = false;
}
</script>

<template>

    <div class="khung">
        <div v-if="loading == true"
            style="width: 100%; height: 100%; background-color: #dddddd50; position: absolute; margin-top: -19.5px; margin-left: -19.5px; z-index: 9999; display: flex; justify-content: center; align-items: center;border-radius: 15px;">
            <div class="spinner-border">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>
        <div>
            <span class="icon-closeForm">
                <a class="custom-link" @click="Close()">
                    <i class="ri-close-line"></i>
                </a>
            </span>
            <h3 v-if="bien == true" style="margin-inline: 27%;">Chọn vị trí hoạt động</h3>
            <h3 v-else style="margin-inline: 27%;">Chọn vị trí nhận xe</h3>
            <div style="display: flex;justify-content: space-between;">
                <button @click="getCurrentLocation" class="btn" style="width: 150px;height: 40px; margin-bottom: 5px;">
                    Vị trí hiện tại <img class="imgPlane" style="margin-left: 5px;" width="20" height="20"
                        src="/src/assets/logoWeb/plane.png" alt="paper-plane--v1" /></button>
                <div
                    style="margin-left: 3px;padding: 7px 8px;border: 0.5px solid #ccc;border-radius: 5px;max-width: 400px;max-height: 37px;overflow: hidden;">
                    <label>Địa chỉ:
                        <i class="i" v-if="address == null">chưa chọn vị trí</i>
                        <i class="i" v-else>{{ address }}</i>
                    </label>
                </div>
            </div>
            <!-- Bản đồ -->
            <div ref="mapContainer" style="width: 650px; height: 550px; position: relative"></div>
            <button class="btn1 nutsubmit" @click="Submit(lat, lng, address)" :disabled="lng == 0 || lat == 0">Xác
                nhận</button>
        </div>

    </div>
</template>
<script>
import MapService from "../../Service/api/MapService";
export default {
    data() {
        return {
            valueLat: 0,
            valueLng: 0
        }
    },
    props: {
        bien: {
            type: Boolean,
            default: false,
            required: false
        },
    },

    methods: {
        Close() {
            this.$emit('Close');
        },
        Submit(lat, lng, address) {
            this.$emit('Submit', lat, lng, address);
        },
    },
    created() {
        console.log(this.bien);
        // this.bienValue = bien;

    },


}
</script>
<style scoped>
/* Import MapLibre CSS nếu chưa import ở script */
@import 'maplibre-gl/dist/maplibre-gl.css';
/* Đặt kích thước và kiểu cơ bản cho container */

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
    right: 0;
    margin-right: 30px;
    margin-top: -80px;
    z-index: 1000;
}

.imgPlane {
    width: 20px !important;
    height: 20px !important;
}
.i{
    opacity: 0.75;
    font-size: 14.5px;
}
</style>