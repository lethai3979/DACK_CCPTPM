<template>
    <div class="khung">
        <div>
            <span class="icon-closeForm">
                <a class="custom-link" @click="Close()">
                    <i class="ri-close-line"></i>
                </a>
            </span>
            <h3 style="margin-inline: 27%;">Chọn vị trí nhận xe</h3>
            <div style="display: flex;justify-content: space-between;">
                <button @click="getCurrentLocation" class="btn" style="width: 150px;height: 40px; margin-bottom: 5px;display: flex;align-items: center;align-content: center;justify-content: center;justify-items: center;">
                    Vị trí hiện tại <ion-icon style="font-size: 22px;margin-left: 2px;" name="navigate-circle-outline"></ion-icon></button>
                <div
                    style="margin-left: 3px;padding: 7px 8px;border: 0.5px solid #ccc;border-radius: 5px;max-width: 400px;max-height: 37px;overflow: hidden;">
                    <label>Địa chỉ:
                        <i class="i" v-if="address == null">chưa chọn vị trí</i>
                        <i class="i" v-else>{{ address }}</i>
                    </label>
                </div>
            </div>
            <!-- Bản đồ -->
            <!-- <div ref="mapContainer" style="width: 650px; height: 550px; position: relative"></div> -->
            <div class="map-container">
                <div ref="map" style="width: 650px; height: 550px;"></div>
            </div>
            <button class="btn1 nutsubmit" @click="Submit()" :disabled="address == null">Xác
                nhận</button>
        </div>

    </div>
</template>

<script>
export default {
    name: 'GoogleMap',
    data() {
        return {
            map: null,
            marker: null,
            selectedLocation: null,
            address: null,
            lat: null,
            lng: null

        };
    },
    mounted() {
        // Chờ Google Maps API sẵn sàng trước khi khởi tạo bản đồ
        const checkGoogleMaps = setInterval(() => {
            if (window.google && window.google.maps) {
                clearInterval(checkGoogleMaps);
                this.initMap();
            }
        }, 100);
    },
    methods: {
        Close() {
            this.$emit('Close');
        },
        Submit() {
            this.$emit('Submit', this.lat, this.lng, this.address);
            // console.log("Address: ",this.address,"Lat: " ,this.lat,"Lng: ", this.lng);
        },
        initMap() {
            // Địa điểm ban đầu (Hồ Chí Minh)
            const initialLocation = { lat: 10.762622, lng: 106.660172 };

            // Tạo bản đồ
            this.map = new google.maps.Map(this.$refs.map, {
                center: initialLocation,
                zoom: 12,
            });

            // Thêm sự kiện click vào bản đồ để đặt marker
            this.map.addListener('click', (event) => {
                const lat = event.latLng.lat();
                const lng = event.latLng.lng();
                this.readMap(lat,lng);
                this.updateMarker(event.latLng);
            });
        },
        readMap(lat,lng){
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
        createMarker(location) {
            // Xóa marker cũ nếu tồn tại
            if (this.marker) {
                this.marker.setMap(null);
            }

            // Tạo marker mới
            this.marker = new google.maps.Marker({
                position: location,
                map: this.map,
                draggable: true,
            });

            // Cập nhật vị trí được chọn
            this.selectedLocation = location;
            this.map.setZoom(16.5);
            // Thêm sự kiện kéo marker
            this.marker.addListener('dragend', (event) => {
                this.updateMarker(event.latLng);
            });
        },

        updateMarker(latLng) {
            // Chuyển đổi latLng sang đối tượng có thể lưu trữ
            const location = {
                lat: latLng.lat(),
                lng: latLng.lng()
            };

            // Tạo hoặc di chuyển marker đến vị trí mới
            if (this.marker) {
                this.marker.setPosition(location);
            } else {
                this.createMarker(location);
            }

            // Cập nhật vị trí được chọn
            this.selectedLocation = location;

            // Di chuyển bản đồ đến vị trí mới
            this.map.panTo(location);
            this.map.setZoom(16.5);
            this.lat = location.lat;
            this.lng = location.lng;
        },

        getCurrentLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    (position) => {
                        const currentLocation = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };
                        this.readMap(currentLocation.lat,currentLocation.lng);
                        // Cập nhật marker tại vị trí hiện tại
                        this.updateMarker(
                            new google.maps.LatLng(currentLocation.lat, currentLocation.lng)
                        );
                        this.map.setZoom(16.5);
                    },
                    (error) => {
                        console.error('Lỗi lấy vị trí:', error);
                        alert('Không thể lấy vị trí của bạn');
                    }
                );
            } else {
                alert('Trình duyệt không hỗ trợ định vị');
            }
        },
        
    }
};
</script>

<style scoped>
.map-container {
    position: relative;
}

.map-controls {
    margin-top: 10px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.btn {
    padding: 10px 15px;
    background-color: #4CAF50;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}

.location-info {
    display: flex;
    gap: 20px;
}

.location-info p {
    margin: 0;
}




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
</style>