<template>
    <div>
        <div class="header" style="">
            <div class="container5 header-container">
                <div class="header-left">
                    <h1>Thuê xe nhanh chóng tiện lợi.</h1>
                    <h3>Phủ sóng 4 thành phố lớn nhất Việt Nam</h3>
                    <p>
                        Với rất nhiều loại xe, hãng xe, tiện nghi bạn có thể tùy chọn, đáp ứng mọi nhu cầu của khách
                        hàng.
                    </p>
                    <a @click="this.$router.push({ name: 'post-search' });" class="btn">Thuê xe ngay</a>
                </div>
                <div class="header-right">
                    <div class="sq-box">
                        <img src="../../assets/logoWeb/car2.png" alt="">
                    </div>
                    <div class="sq-box2"></div>
                </div>
            </div>

        </div>
        <div class="container" style="min-height:765px;margin-bottom:10px;margin-top:10px;">
            <div style="margin-top:30px;margin-left: -7px;">
                <div style="font-size:30px;font-weight:bold;margin-bottom:10px;margin-left: 5px;"> Loại xe nổi bật
                </div>
                <div class="image-slider">
                    <a href="javascript:void(0)" class="prev-button" @click.prevent="trai()">&#10094;</a>
                    <a href="javascript:void(0)" class="next-button" @click.prevent="phai()">&#10095;</a>
                    <div class="slider-container">
                        <a v-for="slide in visibleSlides" :key="slide.value" class="slide">
                            <input :value="slide.value" hidden />
                            <img :src="'/src/assets/logoWeb/' + slide.src" :alt="slide.label">
                            <label>{{ slide.label }}</label>
                        </a>
                    </div>
                </div>
            </div>

            <div style="margin-top:30px;">
                <div style="font-size:30px;font-weight:bold;margin-bottom:10px;"> Địa điểm nổi bật </div>
                <div class="anhaa">
                    <a asp-controller="home" asp-action="Address" data-name="Hà Nội" asp-route-name="Hà Nội"
                        style="margin-left:7px;">
                        <label class="sumxe" id="sum-hanoi">2 xe</label>
                        <img class="anhnb" src="/src/assets/logoWeb/hanoi.png">
                    </a>
                    <a asp-controller="home" asp-action="Address" data-name="Đà Nẵng" asp-route-name="Đà Nẵng">
                        <label class="sumxe" id="sum-danang">3 xe</label>
                        <img class="anhnb" src="/src/assets/logoWeb/danang.png">
                    </a>
                    <a asp-controller="home" asp-action="Address" data-name="TPHCM" asp-route-name="TPHCM">
                        <label class="sumxe" id="sum-tphcm">4 xe</label>
                        <img class="anhnb" src="/src/assets/logoWeb/tphcm.png">
                    </a>
                    <a asp-controller="home" asp-action="Address" data-name="Cà Mau" asp-route-name="Cà Mau">
                        <label class="sumxe" id="sum-camau">5 xe</label>
                        <img class="anhnb" src="/src/assets/logoWeb/camau.png">
                    </a>
                </div>
            </div>


            <div style="margin-top:30px;">
                <div style="font-size:30px;font-weight:bold;margin-bottom:10px;"> Xe gợi ý cho bạn </div>
                <div class="container">
                    <div class="containerPost">

                        <router-link :to="{ name: 'user-post-detail', params: { id: item.id } }" class="box postcar"
                            v-for="item in posts" :key="item.id">
                            <div class="khuyenmailist"
                                v-if="item.postPromotions && item.postPromotions.length > 0 && item.postPromotions[0].promotion.discountValue < 1">
                                Giảm {{ item.postPromotions[0].promotion.discountValue * 100 }}%
                            </div>
                            <div class="khuyenmailist"
                                v-if="item.postPromotions && item.postPromotions.length > 0 && item.postPromotions[0].promotion.discountValue > 1">
                                Giảm {{ formatPrice(item.postPromotions[0].promotion.discountValue) }}
                            </div>
                            <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                                <img :src="'http://localhost:5027/' + item.image" :alt="item.image"
                                    style="object-fit: cover; width: 100%; height: 100%;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);" />
                            </div>
                            <div class="ptb">
                                <div v-if="item.gear" class="ptb1 apt" style="padding:2px 4px;">Số tự động</div>
                                <div v-else class="ptb1 apt" style="padding:2px 4px;background-color:darkkhaki;">Số sàn
                                </div>
                            </div>

                            <div class="ptc">
                                <router-link :to="{ name: 'user-post-detail', params: { id: item.id } }">
                                    {{ item.name }}
                                </router-link>
                                <!-- <a href="">{{ item.name }}</a> -->
                            </div>
                            <div class="ptd">
                                <img src="/src/assets/logoWeb/road-map-line.svg" class="icon_map" alt="">
                                <span class="text_map">{{ item.rentLocation }}</span>
                            </div>
                            <hr>
                            <div class="pte">
                                <div class="pte_left">
                                    <!-- @* Chưa xong *@ -->
                                    <div class="dgsao">
                                        <img src="/src/assets/logoWeb/star-s-fill.svg" class="icon_sao_danhgia" alt="">
                                        <span class="text_saodanhgia">{{ formatDecimal(item.avgRating, 1) }}</span>
                                    </div>

                                    <div v-if="item.rideNumber > 0" class="sochuyen">
                                        <img src="/src/assets/logoWeb/luggage-cart-line.svg" class="icon_map" alt="">
                                        <div class="text_sochuyen">{{ item.rideNumber }} chuyến</div>
                                    </div>

                                    <div v-else class="sochuyen">
                                        <div class="text_sochuyen">Chưa có chuyến</div>
                                    </div>


                                </div>
                                <div class="pte_right">
                                    <div class="giagiam">{{ formatPrice(item.pricePerHour) }}/giờ</div>
                                    <div class="giagiam">{{ formatPrice(item.pricePerDay) }}/ngày</div>
                                </div>
                            </div>
                        </router-link>

                    </div>
                </div>
            </div>


            <div style="margin-top:30px;">
                <div style="font-size:30px;font-weight:bold;margin-bottom:10px;"> Hướng dẫn thuê xe trên trang web
                    GoWheels </div>
                <div style="display:flex">
                    <div class="box1"
                        style="padding:3px;margin-inline:8.7px;background-color:white;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); margin-bottom:15px;">
                        <img style="width: 305px;height: 345px;border-radius: 15px;"
                            src="/src/assets/logoWeb/sss1.png" />
                        <label>
                            1. Tìm kiếm xe theo nhu cầu
                        </label>
                    </div>
                    <div class="box1"
                        style="padding:3px;margin-inline:8.7px;background-color:white;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); margin-bottom:15px;">
                        <img style="width: 305px;height: 345px;border-radius: 15px;"
                            src="/src/assets/logoWeb/sss2.png" />
                        <label>
                            2. Chọn ngày thuê và ngày trả
                        </label>
                    </div>
                    <div class="box1"
                        style="padding:3px;margin-inline:8.7px;background-color:white;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); margin-bottom:15px;">
                        <img style="width: 305px;height: 345px;border-radius: 15px;"
                            src="/src/assets/logoWeb/sss3.png" />
                        <label>
                            3. Đặt cọc xe
                        </label>
                    </div>
                    <div class="box1"
                        style="padding:3px;margin-inline:8.7px;background-color:white;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); margin-bottom:15px;">
                        <img style="width: 305px;height: 345px;border-radius: 15px;"
                            src="/src/assets/logoWeb/sss4.png" />
                        <label>
                            4. Liên hệ với chủ xe và nhận xe
                        </label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Footer -->
    <!-- Footer -->
    <footer class="text-center text-lg-start text-muted" style="background-color: #303030 !important;">
        <!-- Section: Social media -->
        <section class="d-flex justify-content-center justify-content-lg-between p-4 border-bottom" style="color: aliceblue;">
            <!-- Left -->
            <div class="me-5 d-none d-lg-block">
                <span>Kết nối với chúng tôi trên mạng xã hội:</span>
            </div>
            <!-- Left -->

            <!-- Right -->
            <div>
                <a href="" class="me-4 text-reset">
                    <i class="fab fa-facebook-f"></i>
                </a>
                <a href="" class="me-4 text-reset">
                    <i class="fab fa-twitter"></i>
                </a>
                <a href="" class="me-4 text-reset">
                    <i class="fab fa-google"></i>
                </a>
                <a href="" class="me-4 text-reset">
                    <i class="fab fa-instagram"></i>
                </a>
                <a href="" class="me-4 text-reset">
                    <i class="fab fa-linkedin"></i>
                </a>
                <a href="" class="me-4 text-reset">
                    <i class="fab fa-github"></i>
                </a>
            </div>
            <!-- Right -->
        </section>
        <!-- Section: Social media -->

        <!-- Section: Links  -->
        <section class="">
            <div class="container text-center text-md-start mt-5" style="background-color: #303030 !important;color: aliceblue;">
                <!-- Grid row -->
                <div class="row mt-3">
                    <!-- Grid column -->
                    <div class="col-md-3 col-lg-4 col-xl-3 mx-auto mb-4"    >
                        <!-- Content -->
                        <h6 class="text-uppercase fw-bold mb-4" >
                            <i class="fas fa-gem me-3" ></i>GoWheels
                        </h6>
                        <p>
                            Thuê xe nhanh chống tiện lợi với rất nhiều hãng xe, loại xe, đáp ứng mọi nhu cầu của khách
                            hàng
                        </p>
                        <p  class="hover" @click="this.$router.push({ name: 'chinhsach' })">Chính sách về
                            quyền riêng tư</p>
                        <p  class="hover" @click="this.$router.push({ name: 'dieukhoan' })">Điều khoản
                            dịch vụ</p>

                    </div>
                    <!-- Grid column -->

                    <!-- Grid column -->
                    <div class="col-md-2 col-lg-2 col-xl-2 mx-auto mb-4">
                        <!-- Links -->
                        <h6 class="text-uppercase fw-bold mb-4">
                            Hãng xe thịnh hành
                        </h6>
                        <p>
                            <a href="#!" class="text-reset">Mercedes</a>
                        </p>
                        <p>
                            <a href="#!" class="text-reset">Toyota</a>
                        </p>
                        <p>
                            <a href="#!" class="text-reset">VinFast</a>
                        </p>
                        <p>
                            <a href="#!" class="text-reset">Mazda</a>
                        </p>

                    </div>
                    <!-- Grid column -->

                    <!-- Grid column -->
                    <div class="col-md-3 col-lg-2 col-xl-2 mx-auto mb-4">
                        <!-- Links -->
                        <h6 class="text-uppercase fw-bold mb-4">
                            Loại xe thịnh hành
                        </h6>
                        <p>
                            <a href="#!" class="text-reset">SPORT</a>
                        </p>
                        <p>
                            <a href="#!" class="text-reset">OFFROAD</a>
                        </p>
                        <p>
                            <a href="#!" class="text-reset">SEDAN</a>
                        </p>
                        <p>
                            <a href="#!" class="text-reset">SUV</a>
                        </p>
                    </div>
                    <!-- Grid column -->

                    <!-- Grid column -->
                    <div class="col-md-4 col-lg-3 col-xl-3 mx-auto mb-md-0 mb-4">
                        <!-- Links -->
                        <h6 class="text-uppercase fw-bold mb-4">Liên hệ</h6>
                        <p><i class="fas fa-home me-3"></i> Quận 9, TpHCM, VN</p>
                        <p>
                            <i class="fas fa-envelope me-3"></i>
                            gowheels03@gmail.com
                        </p>
                        <p><i class="fas fa-phone me-3"></i> 0935200403</p>
                        <p><i class="fas fa-print me-3"></i> 0935200403</p>
                    </div>
                    <!-- Grid column -->
                </div>
                <!-- Grid row -->
            </div>
        </section>
        <!-- Section: Links  -->

        <!-- Copyright -->
        <div class="text-center p-4" style="background-color: rgba(0, 0, 0, 0.05);color: aliceblue;">
            © 2024 :
            <a class="text-reset fw-bold" href="https://mdbootstrap.com/">GoWheels</a>
        </div>
        <!-- Copyright -->
    </footer>
    <!-- Footer -->


</template>

<script>
import PostVM from '../../Model/PostVM';
import PostService from '../../Service/api/PostService';
export default {
    async created() {
        await this.getAllPost();
    },
    data() {
        return {
            slides: [
                { value: 'SEDAN', src: 'sedan.png', label: 'SEDAN' },
                { value: 'OFFROAD', src: 'xebantai.png', label: 'OFFROAD' },
                { value: 'ELECTRIC', src: 'xedien.png', label: 'ELECTRIC' },
                { value: 'SUV', src: 'SUV.png', label: 'SUV' },
                { value: 'SPORT', src: 'sport.png', label: 'SPORT' },
                { value: 'HATCHBACK', src: 'hatchback.png', label: 'HATCHBACK' },
                { value: 'COUPE', src: 'coupe.png', label: 'COUPE' }
            ],
            currentIndex: 0,
            posts: [new PostVM()],
        }
    },
    computed: {

        visibleSlides() {
            const visibleCount = 4;
            const totalSlides = this.slides.length;

            // Tạo mảng slides hiển thị, đảm bảo luôn có 4 slides
            let displaySlides = [];
            for (let i = 0; i < visibleCount; i++) {
                let index = (this.currentIndex + i) % totalSlides;
                displaySlides.push(this.slides[index]);
            }

            return displaySlides;
        }
    },
    methods: {
        formatPrice(price) {
            if (price >= 1000000) {
                return (price / 1000000).toFixed(1).replace('.0', '') + "Tr";
            } else if (price >= 1000) {
                return (price / 1000).toLocaleString('en').replace(/,/g, '.') + "k";
            } else {
                return price;
            }
        },
        formatDecimal(number, decimalPlaces) {
            const factor = Math.pow(10, decimalPlaces);
            return Math.ceil(number * factor) / factor;
        },
        trai() {
            // Di chuyển slide sang trái (quay về slide trước)
            this.currentIndex = (this.currentIndex - 1 + this.slides.length) % this.slides.length;
        },
        phai() {
            // Di chuyển slide sang phải (chuyển sang slide tiếp theo)
            this.currentIndex = (this.currentIndex + 1) % this.slides.length;
        },
        async getAllPost() {
            const response = await PostService.getAllPost();
            if (response.success) {
                console.log(response.data);
                this.posts = response.data.listPost;
            }
        }
    },

}
</script>

<style>
.footer {
    background-color: #1d1d1d;
}

.nutchuyentrang {
    margin-inline: 43%;
    width: 300px;
}

.nutchuyentrang1 {
    margin-left: 75%;
    width: 300px;
}

.khuyenmailist {
    padding: 1px 5px;
    border: 3px solid white;
    border-radius: 50px;
    position: absolute;
    color: var(--color-white1);
    font-size: 14px;
    background-color: #f26a2b;
    right: 0;
    margin-right: 15px;
}
</style>