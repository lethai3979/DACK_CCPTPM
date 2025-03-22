<template>
    <div id="main-wrapper">
        <div id="content">
            <section class="container">

                <div class="row">

                    <!-- Side Panel
        ============================================= -->
                    <aside class="col-lg-3">
                        <div class="bg-white shadow-md rounded p-3">
                            <h3 class="text-5">Lọc</h3>
                            <hr class="mx-n3">
                            <div class="accordion accordion-flush style-2 mt-n3">
                                <div class="accordion-item">
                                    <h2 class="accordion-header" id="carType" @click="a = !a">
                                        <button class="accordion-button" type="button">Hãng xe</button>
                                    </h2>
                                    <div v-if="a == true" class="accordion-collapse collapse show"
                                        aria-labelledby="carType">
                                        <div class="accordion-body">
                                            <div class="form-check" v-for="option in companies" :key="option">
                                                <input type="checkbox" :id="option.value" name="carType"
                                                    class="form-check-input" :checked="search.company === option.name"
                                                    @change="selectCompany(option.name)">
                                                <label class="form-check-label d-block" :for="option.value">{{
                                                    option.name }}</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="accordion-item">
                                    <h2 @click="b = !b" class="accordion-header" id="passengers">
                                        <button class="accordion-button" type="button">Số chổ ngồi</button>
                                    </h2>
                                    <div v-if="b == true" class="accordion-collapse collapse show"
                                        aria-labelledby="passengers">
                                        <div class="accordion-body">
                                            <div class="form-check" v-for="option in seatOptions" :key="option.value">
                                                <input type="checkbox" :id="option.value" name="passengers"
                                                    class="form-check-input" :checked="search.seat === option.value"
                                                    @change="selectSeat(option.value)">
                                                <label class="form-check-label d-block" :for="option.value">{{
                                                    option.label }}</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="accordion-item">
                                    <h2 @click="c = !c" class="accordion-header" id="bags">
                                        <button class="accordion-button" type="button">Loại hộp số</button>
                                    </h2>
                                    <div v-if="c == true" class="accordion-collapse collapse show"
                                        aria-labelledby="bags">
                                        <div class="accordion-body">
                                            <div class="form-check">
                                                <input @change="search.gear = true" :checked="search.gear === true"
                                                    type="checkbox" id="1to2" name="bags" class="form-check-input">
                                                <label class="form-check-label d-block" for="1to2">Số tự động</label>
                                            </div>
                                            <div class="form-check">
                                                <input @change="search.gear = false" type="checkbox"
                                                    :checked="search.gear === false" id="1to2" name="bags"
                                                    class="form-check-input">
                                                <label class="form-check-label d-block" for="1to2">Số sàn</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="accordion-item">
                                    <h2 @click="d = !d" class="accordion-header" id="transmission">
                                        <button class="accordion-button" type="button">Loại nhiên liệu</button>
                                    </h2>
                                    <div v-if="d == true" class="accordion-collapse collapse show"
                                        aria-labelledby="bags">
                                        <div class="accordion-body">
                                            <div class="form-check">
                                                <input type="checkbox" @change="search.fuel = 'Gasoline'"
                                                    :checked="search.fuel == 'Gasoline'" id="paynow" name="transmission"
                                                    class="form-check-input">
                                                <label class="form-check-label d-block" for="paynow">Xăng</label>
                                            </div>
                                            <div class="form-check">
                                                <input type="checkbox" @change="search.fuel = 'Diesel'"
                                                    :checked="search.fuel == 'Diesel'" id="paynow" name="transmission"
                                                    class="form-check-input">
                                                <label class="form-check-label d-block" for="paynow">Dầu</label>
                                            </div>
                                            <div class="form-check">
                                                <input type="checkbox" @change="search.fuel = 'Electric'"
                                                    :checked="search.fuel == 'Electric'" id="paynow" name="transmission"
                                                    class="form-check-input">
                                                <label class="form-check-label d-block" for="paynow">Điện</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="accordion-item">
                                    <h2 @click="e = !e" class="accordion-header" id="userReview">
                                        <button class="accordion-button" type="button">Có tài xế</button>
                                    </h2>
                                    <div v-if="e == true" class="accordion-collapse collapse show"
                                        aria-labelledby="userReview">
                                        <div class="accordion-body">
                                            <div class="form-check">
                                                <input @change="search.hasDriver = true" type="checkbox" id="excellent"
                                                    name="userReview" class="form-check-input">
                                                <label class="form-check-label d-block" for="excellent">Xe có tài
                                                    xế</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="margin-top: 20px;" @click="searchPostV2()">
                                    <button class="btn btn-primary" type="submit">Tìm kiếm</button>
                                </div>
                            </div>
                        </div>
                    </aside>
                    <!-- Side Panel end -->

                    <div class="col-lg-9 mt-4 mt-lg-0">

                        <!-- Sort Filters
          ============================================= -->
                        <div class="border-bottom mb-3 pb-3">
                            <div class="row align-items-center">
                                <div class="col-sm-6 col-md-8">
                                    <h2>Tìm kiếm xe cho bạn</h2>
                                </div>
                                <!-- <span class="me-3"><span class="text-4">Ahmedabad:</span> <span class="fw-600">215 Cars</span> found</span> <span class="text-warning text-nowrap">Prices inclusive of taxes</span> -->
                                <div class="col-sm-6 col-md-4">
                                    <div class="row g-0 ms-auto">
                                        <label class="col col-form-label-sm text-end me-2 mb-0" for="input-sort">Sắp xếp
                                            theo :</label>
                                        <select id="input-sort" @change="SortPost($event.target.value)"
                                            class="form-select form-select-sm col">
                                            <option value="0" selected="selected">Mặc định</option>
                                            <option value="1">Giá cao nhất</option>
                                            <option value="2">Giá thấp nhất</option>
                                            <option value="3">Lượt thuê cao nhất</option>
                                            <option value="4">Đánh giá tốt nhất</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Sort Filters end -->

                        <!-- List Item
          ============================================= -->
                        <div class="car-list" style="min-height: 540px;" v-if="posts.length > 0">
                            <div class="car-item bg-white shadow-md rounded p-3" style="margin-bottom: 10px;"
                                v-for="item in posts" :key="item">
                                <div class="row">
                                    <div class="col-md-4"> <a href="#"><img class="img-fluid rounded align-top"
                                                :src="'http://localhost:5027/' + item.image" alt="cars"></a> </div>
                                    <div class="col-md-8 mt-3 mt-md-0">
                                        <div class="row g-0">
                                            <div class="col-sm-9">
                                                <h4 class="d-flex align-items-center"><a href="#"
                                                        class="text-dark text-5 me-2">{{ item.name }}</a>
                                                    <span v-if="item.gear"
                                                        class="alert alert-info rounded-pill px-2 py-1 lh-1 fw-400 text-2 mb-0">{{
                                                            item.carTypeName }}</span>
                                                </h4>
                                                <p class="car-features d-flex align-items-center mb-2 text-4">
                                                    <span data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="5 Adult Passenger">
                                                        <img width="15" height="15"
                                                            style="opacity: 0.86;margin-top: -2px;"
                                                            src="/src/assets/logoWeb/seatbelt.png" alt="car-seat" />
                                                        <small class="text-2">{{ item.seat }}</small>
                                                    </span>
                                                    <span data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="2 Large Bag"><i
                                                            class="fas fa-suitcase-rolling"></i>
                                                        <small class="text-2">{{ item.rideNumber }}</small>
                                                    </span>
                                                    <span data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="Manual transmission"><i
                                                            class="fas fa-cog"></i>
                                                        <small v-if="item.gear" class="text-2">Số tự động</small>
                                                        <small v-else class="text-2">Số sàn</small>
                                                    </span>

                                                </p>
                                                <div class="row text-1 mb-3">
                                                    <div v-for="(jtem, index) in item.postAmenities.slice(0, 8)"
                                                        :key="index" data-bs-toggle="tooltip" title="" class="col-6"
                                                        data-bs-original-title="Free cancellation up to 72 hours prior to pick up">
                                                        <span class="text-success me-1">
                                                            <i class="fas fa-check"></i>
                                                        </span>
                                                        {{ jtem.amenityName }}
                                                    </div>
                                                </div>
                                                <p v-if="item.avgRating > 0" class="reviews mb-0">
                                                    <!--<span
                                                        class="reviews-score px-2 py-1 rounded fw-600 text-white">4.7</span> -->
                                                    <span class="fw-600">Đánh giá: </span> <a class="text-muted"
                                                        href="#">{{ formatDecimal(item.avgRating, 1) }} <img
                                                            style="margin-top: -5px;" width="20" height="20"
                                                            src="https://img.icons8.com/fluency/48/star--v1.png"
                                                            alt="star--v1" />
                                                    </a>
                                                </p>
                                                <p v-else class="reviews mb-0">
                                                    <!--<span
                                                        class="reviews-score px-2 py-1 rounded fw-600 text-white">4.7</span> -->
                                                    <span class="fw-600">Đánh giá: </span> <a class="text-muted"
                                                        href="#">Chưa có đánh giá</a>
                                                </p>
                                            </div>
                                            <div class="col-sm-3 text-end d-flex d-sm-block align-items-center">
                                                <div v-if="item.postPromotions && item.postPromotions.length > 0 && item.postPromotions[0].promotion.discountValue < 1" class="text-success text-3 mb-0 mb-sm-1 order-2 ">Giảm {{ item.postPromotions[0].promotion.discountValue * 100 }}%</div>
                                                <div v-if="item.postPromotions && item.postPromotions.length > 0 && item.postPromotions[0].promotion.discountValue > 1" class="text-success text-3 mb-0 mb-sm-1 order-2 ">Giảm {{ formatPrice(item.postPromotions[0].promotion.discountValue) }}đ</div>
                                                <div class="text-dark text-7 fw-500 mb-0 mb-sm-2 me-2 me-sm-0 order-0">
                                                    {{ formatPrice(item.pricePerHour) }}/giờ</div>
                                                <div class="text-dark text-7 fw-500 mb-0 mb-sm-2 me-2 me-sm-0 order-0">
                                                    {{ formatPrice(item.pricePerDay) }}/ngày</div>
                                                <a @click="this.$router.push({ name: 'user-post-detail', params: { id: item.id } });"
                                                    class="btn">Đặt xe</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="car-list" style="min-height: 540px;" v-else>
                            <div class="car-item bg-white shadow-md rounded p-3" style="margin-bottom: 10px;"
                                v-for="item in posts" :key="item">
                                <div class="row">
                                    <div class="col-md-4"> <a href="#"><img class="img-fluid rounded align-top"
                                                :src="'http://localhost:5027/' + item.image" alt="cars"></a> </div>
                                    <div class="col-md-8 mt-3 mt-md-0">
                                        <div class="row g-0">
                                            <div class="col-sm-9">
                                                <h4 class="d-flex align-items-center"><a href="#"
                                                        class="text-dark text-5 me-2">{{ item.name }}</a>
                                                    <span v-if="item.gear"
                                                        class="alert alert-info rounded-pill px-2 py-1 lh-1 fw-400 text-2 mb-0">{{
                                                            item.carTypeName }}</span>

                                                </h4>
                                                <p class="car-features d-flex align-items-center mb-2 text-4">
                                                    <span data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="5 Adult Passenger">
                                                        <img width="15" height="15"
                                                            style="opacity: 0.86;margin-top: -2px;"
                                                            src="/src/assets/logoWeb/seatbelt.png" alt="car-seat" />
                                                        <small class="text-2">{{ item.seat }}</small>
                                                    </span>
                                                    <span data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="2 Large Bag"><i
                                                            class="fas fa-suitcase-rolling"></i>
                                                        <small class="text-2">{{ item.rideNumber }}</small>
                                                    </span>
                                                    <span data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="Manual transmission"><i
                                                            class="fas fa-cog"></i>
                                                        <small v-if="item.gear" class="text-2">Số tự động</small>
                                                        <small v-else class="text-2">Số sàn</small>
                                                    </span>

                                                </p>
                                                <div class="row text-1 mb-3">
                                                    <div v-for="(jtem, index) in item.postAmenities.slice(0, 8)"
                                                        :key="index" data-bs-toggle="tooltip" title="" class="col-6"
                                                        data-bs-original-title="Free cancellation up to 72 hours prior to pick up">
                                                        <span class="text-success me-1">
                                                            <i class="fas fa-check"></i>
                                                        </span>
                                                        {{ jtem.amenityName }}
                                                    </div>
                                                </div>
                                                <p v-if="item.avgRating > 0" class="reviews mb-0">
                                                    <!--<span
                                                        class="reviews-score px-2 py-1 rounded fw-600 text-white">4.7</span> -->
                                                    <span class="fw-600">Đánh giá: </span> <a class="text-muted"
                                                        href="#">{{ formatDecimal(item.avgRating, 1) }} <img
                                                            style="margin-top: -5px;" width="20" height="20"
                                                            src="https://img.icons8.com/fluency/48/star--v1.png"
                                                            alt="star--v1" />
                                                    </a>
                                                </p>
                                                <p v-else class="reviews mb-0">
                                                    <!--<span
                                                        class="reviews-score px-2 py-1 rounded fw-600 text-white">4.7</span> -->
                                                    <span class="fw-600">Đánh giá: </span> <a class="text-muted"
                                                        href="#">Chưa có đánh giá</a>
                                                </p>
                                            </div>
                                            <div class="col-sm-3 text-end d-flex d-sm-block align-items-center">
                                                <div class="text-dark text-7 fw-500 mb-0 mb-sm-2 me-2 me-sm-0 order-0">
                                                    {{ formatPrice(item.pricePerHour) }}/giờ</div>
                                                <div class="text-dark text-7 fw-500 mb-0 mb-sm-2 me-2 me-sm-0 order-0">
                                                    {{ formatPrice(item.pricePerDay) }}/ngày</div>
                                                <a href="#" class="btn">Book Now</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- <div class="car-item bg-white shadow-md rounded p-3">
                                <div class="row">
                                    <div class="col-md-4"> <a href="#"><img class="img-fluid rounded align-top"
                                                src="/src/assets/logoWeb/8.png" alt="cars"></a> </div>
                                    <div class="col-md-8 mt-3 mt-md-0">
                                        <div class="row g-0">
                                            <div class="col-sm-9">
                                                <h4 class="d-flex align-items-center"><a href="#"
                                                        class="text-dark text-5 me-2">Innova Crysta</a> <span
                                                        class="alert alert-info rounded-pill px-2 py-1 lh-1 fw-400 text-2 mb-0">Economy</span>
                                                </h4>
                                                <p class="car-features d-flex align-items-center mb-2 text-4"> <span
                                                        data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="7 Adult Passenger"><i
                                                            class="fas fa-user"></i> <small
                                                            class="text-2">7</small></span> <span
                                                        data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="1 Large Bag"><i
                                                            class="fas fa-suitcase-rolling"></i> <small
                                                            class="text-2">1</small></span> <span
                                                        data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="1 Small Bag"><i
                                                            class="fas fa-suitcase"></i> <small
                                                            class="text-2">1</small></span> <span
                                                        data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="Manual transmission"><i
                                                            class="fas fa-cog"></i> <small
                                                            class="text-2">Manual</small></span> <span
                                                        data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="Drive unlimited distance with this car at no extra cost"><i
                                                            class="fas fa-tachometer-alt"></i> <small
                                                            class="text-2">Mileage</small></span> <span
                                                        data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="Air Conditioning Available"><i
                                                            class="fas fa-snowflake"></i> <small
                                                            class="text-2">A/C</small></span> </p>
                                                <div class="row text-1 mb-3">
                                                    <div data-bs-toggle="tooltip" title="" class="col-6"
                                                        data-bs-original-title="Free cancellation up to 72 hours prior to pick up">
                                                        <span class="text-success me-1"><i
                                                                class="fas fa-check"></i></span>Free Cancellation
                                                    </div>
                                                    <div class="col-6" data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="Instantly confirmed upon booking"> <span
                                                            class="text-success me-1"><i
                                                                class="fas fa-check"></i></span>Instantly Confirmed
                                                    </div>
                                                    <div class="col-6" data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="In the unlikely event you find a better price on the same brand, we'll beat it. See 'Price Promise' on our About Us page">
                                                        <span class="text-success me-1"><i
                                                                class="fas fa-check"></i></span>Price Guarantee
                                                    </div>
                                                    <div class="col-6" data-bs-toggle="tooltip" title=""
                                                        data-bs-original-title="Rate includes Third Party Liability Cover">
                                                        <span class="text-success me-1"><i
                                                                class="fas fa-check"></i></span>Third Party Liability
                                                    </div>
                                                </div>
                                                <p class="reviews mb-0"> <span
                                                        class="reviews-score px-2 py-1 rounded fw-600 text-white">3.9</span>
                                                    <span class="fw-600">Good</span> <a class="text-muted" href="#">(256
                                                        reviews)</a>
                                                </p>
                                            </div>
                                            <div class="col-sm-3 text-end d-flex d-sm-block align-items-center">
                                                <div class="text-success text-3 mb-0 mb-sm-1 order-2 ">10% Off!</div>
                                                <div
                                                    class="d-block text-3 text-muted mb-0 mb-sm-2 me-2 me-sm-0 order-1">
                                                    <del class="d-block">$390</del>
                                                </div>
                                                <div class="text-dark text-7 fw-500 mb-0 mb-sm-2 me-2 me-sm-0 order-0">
                                                    $351</div>
                                                <div class="text-muted mb-0 mb-sm-2 order-3 d-none d-sm-block">per day
                                                </div>
                                                <a href="#" class="btn btn-sm btn-primary order-4 ms-auto">Book Now</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div> -->

                        </div>
                        <!-- Pagination
            ============================================= -->
                        <!-- <ul class="pagination justify-content-center mt-4 mb-0">
                            <li class="page-item disabled"> <a class="page-link" href="#" tabindex="-1"><i
                                        class="fas fa-angle-left"></i></a> </li>
                            <li class="page-item"><a class="page-link" href="#">1</a></li>
                            <li class="page-item active"> <a class="page-link" href="#">2</a> </li>
                            <li class="page-item"><a class="page-link" href="#">3</a></li>
                            <li class="page-item"> <a class="page-link" href="#"><i class="fas fa-angle-right"></i></a>
                            </li>
                        </ul> -->
                        <ul class="pagination justify-content-center mt-4 mb-0">
                            <!-- Nút "Trở về" -->
                            <li :class="['page-item', { disabled: currentPage === 1 }]">
                                <a class="page-link" href="#" @click.prevent="changePage(currentPage - 1)">
                                    <i class="fas fa-angle-left"></i>
                                </a>
                            </li>

                            <!-- Các số trang -->
                            <li v-for="page in totalPages" :key="page"
                                :class="['page-item', { active: currentPage === page }]">
                                <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
                            </li>

                            <!-- Nút "Tiếp theo" -->
                            <li :class="['page-item', { disabled: currentPage === totalPages }]">
                                <a class="page-link" href="#" @click.prevent="changePage(currentPage + 1)">
                                    <i class="fas fa-angle-right"></i>
                                </a>
                            </li>
                        </ul>
                        <!-- Paginations end -->

                    </div>
                </div>
            </section>
        </div>
        <!-- Content end -->

    </div>
</template>

<script>
import UserVM from '../../Model/UserVM';
import PostVM from '../../Model/PostVM';
import SearchDto from '../../DTOs/SearchDto';
import PostService from '../../Service/api/PostService';
import SearchService from '../../Service/api/SearchService';
import AuthenticationService from '../../Service/api/AuthenticationService';
import { inject, ref } from 'vue';
export default {
    setup() {
        const companies = inject('companies', ref([]));
        // console.log("Companies của IndexVue: ", companies.value);
        return { companies }
    },
    data() {
        return {
            a: false,
            b: false,
            c: false,
            d: false,
            e: false,
            seatOptions: [
                { value: 'default', label: 'Mặc định' },
                { value: '2', label: '2' },
                { value: '4', label: '4' },
                { value: '5', label: '5' },
                { value: '7', label: '7' },
                { value: '9', label: '9' },
            ],
            user: new UserVM(),
            posts: [new PostVM()],
            postRS: [new PostVM()],
            postdemo: null,
            search: new SearchDto(),
            currentPage: 1, // Trang hiện tại
            totalPosts: 0, // Tổng số bài viết
            postsPerPage: 8, // Số bài viết mỗi trang
        }
    },
    computed: {
        // Tính tổng số trang
        totalPages() {
            return Math.ceil(this.totalPosts / this.postsPerPage);
        },
    },
    methods: {
        SortPost(item) {
            if (item == 1) {
                this.posts.sort((a, b) => b.pricePerHour - a.pricePerHour);
            }
            else if (item == 2) {
                this.posts.sort((a, b) => a.pricePerHour - b.pricePerHour);
            }
            else if (item == 3) {
                this.posts.sort((a, b) => b.rideNumber - a.rideNumber);
            }
            else if (item == 4) {
                this.posts.sort((a, b) => b.avgRating - a.avgRating);
            }
            else if (item == 0) {
                this.posts = this.postRS;
            }
        },
        async searchPost() {
            const response = await SearchService.getAll(this.search, this.currentPage);
            console.log("Giá trị trả về: ", response);
            if (response.success) {
                this.posts = response.data.listPost;
                this.postRS = [...response.data.listPost];
                this.totalPosts = response.data.totalByFilter;
            }
        },
        async searchPostV2() {
            this.currentPage = 1;
            await this.searchPost();
        },
        changePage(page) {
            if (page < 1 || page > this.totalPages) return;
            this.currentPage = page;
            this.searchPost();
        },
        selectSeat(value) {
            this.search.seat = value;
            console.log("Search có giá trị: ", this.search);
        },
        selectCompany(value) {
            this.search.company = value;
            console.log("Search có giá trị: ", this.search);
        },

        formatPrice(value) {
            return new Intl.NumberFormat('vi-VN').format(value);
        },
        async getUser() {
            const response = await AuthenticationService.getUser();
            this.user = response;
        },
        async getAllPost() {
            const response = await PostService.getAllPost();
            this.posts = response.data.listPost;
            this.postRS = [...response.data.listPost];
            this.totalPosts = response.data.totalByFilter;
        },
        formatDecimal(number, decimalPlaces) {
            const factor = Math.pow(10, decimalPlaces);
            return Math.ceil(number * factor) / factor;
        },
        getposts() {
            console.log("Post demo: ", this.posts);
        }
    },
    async created() {
        // await this.getAllPost();
        await this.searchPost();
        await this.getUser();
    },


}
</script>

<style>
.text-2 {
    margin-inline: 2.5px !important;
}

small {
    margin-left: 2px;
}
</style>