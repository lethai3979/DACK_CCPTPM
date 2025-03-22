<template>
    <Message ref="message" />
    <div v-if="showMap == true" class="Login">
        <MapV2 :show="showMap" @Close="Close" @Submit="Submit" />
    </div>
    <div class="">
        <div class="booking" style="padding:20px;" v-if="post != null">
            <div style="display: flex;gap: 20px;">
                <h3 style="font-size:30px;" class="giagiam">{{ formatPrice(post.pricePerHour) }}/Giờ</h3>
                <h3 style="font-size:30px;" class="giagiam">{{ formatPrice(post.pricePerDay) }}/Ngày</h3>
            </div>
            <form @submit.prevent="submitForm">
                <div class="date-time-form">
                    <div class="form-item" @click="ClickDay()">
                        <label for="startDate1">Ngày nhận xe:</label>
                        <label v-if="booking.recieveOn" for="">{{ booking.recieveOn }}</label>
                        <label v-else for="">Chưa chọn ngày</label>
                    </div>
                    <div class="line"></div>
                    <div class="form-item" @click="ClickDay()">
                        <label for="endDate1">Ngày trả xe:</label>
                        <label v-if="booking.returnOn" for="">{{ booking.returnOn }}</label>
                        <label v-else for="">Chưa chọn ngày</label>
                    </div>
                </div>

                <!-- Map -->
                <div class="date-time-form">
                    <div class="form-item" @click="showMap = true">
                        <label for="startDate1">Vị trí nhận xe:</label>
                        <label v-if="address != null" for="">{{ address }}</label>
                        <label v-else>Chưa chọn vị trí</label>
                    </div>
                </div>



                <div v-if="boolHour == true">
                    <!-- <div class="form-check">
                        <label class="form-check-label">
                            <input type="checkbox" class="form-check-input" v-model="booking.isRequireDriver" id="Total"
                                :checked="booking.isRequireDriver">
                            Đặt tài xế cho chuyến đi
                        </label>
                    </div> -->
                    <div style="display: flex;gap: 20px;"
                        v-if="post.hasDriver == false && isDayMoreThan3(booking.recieveOn) == true">
                        <label style="margin-left: 10px;" class="circle-checkbox">
                            <input type="checkbox" v-model="booking.isRequireDriver" id="Total"
                                :checked="booking.isRequireDriver">
                            <span class="custom-circle"></span>
                        </label>
                        <label style="margin-left: -10px;">Đặt tài xế cho chuyến đi</label>
                    </div>
                    <!-- <div class="form-group">
                        <label for="Total" class="control-label">Đặt tài xế cho chuyến đi</label>
                        <input v-model="booking.isRequireDriver" id="Total" type="checkbox" :checked="booking.isRequireDriver" class="form-" readonly
                             />
                        <span for="Total" class="text-danger"></span>
                    </div> -->
                    <div class="form-group priceBooking">
                        <label for="Total" class="control-label">Tổng cộng</label>
                        <!-- <input v-model="booking.total" id="Total" class="form-control" readonly
                            style="border:none;background-color:white;" /> -->
                        <span for="Total" class="">{{ formatMoney(booking.total) }}đ</span>
                    </div>
                    <div class="form-group">
                        <label for="PromotionId" class="control-label">Khuyến mãi: </label>
                        <div class="khuyenmai" v-if="post.postPromotions && post.postPromotions.length > 0"
                            @click="ShowVouCherPost()" @change="userPromotion = 1">
                            <div class="khuyenmai_trai z">
                                <label class="circle-checkbox ">
                                    <input type="checkbox" disabled :checked="userPromotion == 1">
                                    <span class="custom-circle"></span>
                                </label>
                                <img width="30" height="30" src="../../../../assets/logoWeb/voucherDo.png"
                                    alt="discount-ticket" />
                            </div>
                            <div class="khuyenmai_phai" v-if="post.postPromotions != []">
                                <label style="display: flex; justify-content: space-between;" for="">
                                    <div style="margin-top: -5px;">Giảm giá của xe</div>
                                    <img style="width: 15px; height: 15px;"
                                        v-if="post.postPromotions && post.postPromotions.length >= 2"
                                        :hidden="userKM == true"
                                        src="https://img.icons8.com/material-rounded/24/chevron-right.png"
                                        alt="chevron-right" />
                                    <label class="x" v-if="userKM == true" @click.stop="XoaKM()">x</label>
                                </label>
                                <label v-if="userKM == true" class="sale"
                                    style="display: flex;justify-content: space-between;">
                                    <label v-if="promotion.discountValue < 1" for="" style="left: 0 !important;"> Khuyến
                                        mãi
                                        {{ promotion.discountValue * 100 }}%</label>
                                    <label v-else for="" style="left: 0 !important;"> Khuyến mãi {{
                                        format(promotion.discountValue) + "K" }}</label>
                                    <label v-if="promotion.discountValue < 1">- {{ formatMoney(booking.total *
                                        promotion.discountValue)
                                        }}</label>
                                    <label v-else>- {{ formatMoney(promotion.discountValue) }}</label>
                                </label>
                            </div>
                            <div class="khuyenmai_phai" v-else>
                                <label for="">
                                    <div>Bài viết không có khuyến mãi</div>
                                </label>
                                <label></label>
                            </div>
                        </div>
                        <div class="khuyenmai" @change="userPromotion = 2" @click="ShowVouCherAdmin()">
                            <div class="khuyenmai_trai z">
                                <label class="circle-checkbox ">
                                    <input type="checkbox" disabled :checked="userPromotion == 2">
                                    <span class="custom-circle"></span>
                                </label>
                                <img width="30" height="30" src="../../../../assets/logoWeb/voucherXanh.png"
                                    alt="discount-ticket" />
                            </div>
                            <div class="khuyenmai_phai">
                                <label style="display: flex; justify-content: space-between;" for="">
                                    <div style="margin-top: -5px;">Chương trình giảm giá</div>
                                    <img v-if="adminKM == false" style="width: 15px; height: 15px;"
                                        src="https://img.icons8.com/material-rounded/24/chevron-right.png"
                                        alt="chevron-right" />
                                    <label class="x" v-else @click.stop="XoaKM()">x</label>
                                </label>
                                <!-- <label v-if="adminKM == true" class="sale" style="display: flex;justify-content: space-between;">
                                    <label v-if="promotion.discountValue < 1" for="" style="left: 0 !important;"> Khuyến
                                        mãi
                                        {{ promotion.discountValue * 100 }}%</label>
                                    <label v-else for="" style="left: 0 !important;"> Khuyến mãi {{
                                        format(promotion.discountValue) + "K" }}</label>
                                    <label for="">200304</label>
                                </label> -->
                                <label v-if="adminKM == true" class="sale"
                                    style="display: flex;justify-content: space-between;">
                                    <label v-if="promotion.discountValue < 1" for="" style="left: 0 !important;"> Khuyến
                                        mãi
                                        {{ promotion.discountValue * 100 }}%</label>
                                    <label v-else for="" style="left: 0 !important;"> Khuyến mãi {{
                                        format(promotion.discountValue) + "K" }}</label>
                                    <label v-if="promotion.discountValue < 1">- {{ formatMoney(booking.total *
                                        promotion.discountValue)
                                        }}</label>
                                    <label v-else>- {{ formatMoney(promotion.discountValue) }}</label>
                                </label>
                            </div>

                        </div>
                    </div>
                    <div class="form-group priceBooking">
                        <label asp-for="Total" class="control-label">Thành tiền</label>
                        <!-- <input v-model="booking.finalValue" class="form-control" id="finalValue" readonly
                            style="border:none;background-color:white;" /> -->
                        <span for="FinalValue" class="">{{ formatMoney(booking.finalValue) }}đ</span>
                    </div>
                    <div class="form-group priceBooking">
                        <label asp-for="Total" class="control-label">Đặt cọc</label>
                        <!-- <input v-model="booking.prePayment" class="form-control" id="prePayment" readonly
                            style="border:none;background-color:white;" /> -->
                        <span for="PrePayment" class="">{{ formatMoney(booking.prePayment) }}đ</span>
                    </div>
                    <div class="form-group priceBooking" v-if="remaining > 0">
                        <label asp-for="Total" class="control-label">Còn lại </label>
                        <!-- <input :value="remaining" class="form-control" id="prePayment"
                            style="border:none;background-color:white;" readonly /> -->
                        <span for="PrePayment" class="">{{ formatMoney(remaining) }}đ</span>
                    </div>
                </div>
                <div class="form-group">
                    <input style="margin-top:20px;" type="submit" value="Thuê xe" class="btn btn-primary w" />
                </div>
            </form>
        </div>
    </div>


</template>

<script>
import axios from 'axios';
import PostVM from '../../../../Model/PostVM';
import BookingDto from '../../../../DTOs/BookingDto';
import PromotionVM from '../../../../Model/PromotionVM';
import Message from '../../../../Message.vue';
import PromotionService from '../../../../Service/api/PromotionService';
import PostService from '../../../../Service/api/PostService';
import BookingService from '../../../../Service/api/BookingService';
import Map from '../../../Home/Map.vue';
import MapV2 from '../../../Home/MapV2.vue';
import MapService from '../../../../Service/api/MapService';
export default {
    components: {
        Message, Map, MapV2
    },
    watch: {
        showMap(newVal) {
            // Thay đổi class của body khi login thay đổi
            if (newVal == true) {
                document.body.classList.add("no-scroll");
            } else {
                document.body.classList.remove("no-scroll");
            }
        },
    },
    props: {
        postId: {
            type: Number,
            required: true
        }
    },
    data() {
        return {
            authen: false,
            post: new PostVM(), // đã có khi bắt đầu chạy
            booking: new BookingDto(),
            promotion: new PromotionVM(),
            hour: false,
            boolHour: false,
            sumhour: 0,
            userKMAd: false,
            userKM: false,
            adminKM: false,
            userPromotion: 0,
            remaining: 0,
            showMap: false,
            location: null,
            address: null
        }
    },
    methods: {
        ToTal() {
            if (this.post != []) {
                if (this.sumhour % 24 == 0) {
                    this.booking.total = (this.sumhour / 24) * this.post.pricePerDay;
                }
                else {
                    this.booking.total = this.sumhour * this.post.pricePerHour;
                }
                if (this.promotion != null && this.promotion.discountValue > 0) {
                    if (this.promotion.discountValue > 1) {
                        this.booking.finalValue = this.booking.total - this.promotion.discountValue;
                    }
                    else {
                        this.booking.finalValue = this.booking.total * (1 - this.promotion.discountValue);
                    }
                }
                else {
                    this.booking.finalValue = this.booking.total;
                }
                this.booking.finalValue = Math.ceil(this.booking.finalValue);
                this.booking.prePayment = this.booking.finalValue / 2;
                this.remaining = this.booking.finalValue - this.booking.prePayment;
                if (this.booking.finalValue < 0) {
                    this.booking.finalValue = 0;
                    this.booking.prePayment = 0;
                    this.remaining = 0;
                }
            }
        },

        async submitForm() {
            console.log("Sau khi submit: ", this.booking);
            if (this.boolHour == false && this.authen == true) {
                this.open("Vui lòng chọn giờ đặt xe");
                return;
            }
            if (this.authen == false) {
                this.open("Vui lòng đăng nhập để đặt xe")
            }
            if (this.authen == true && this.booking.latitude == null || this.authen == true && this.booking.longitude == null) {
                this.open("Vui lòng chọn vị trí nhận xe");
            }
            if (this.boolHour !== false && this.authen !== false) {
                //bắt đầu gửi đi 
                const response = await BookingService.AddBooking(this.booking);
                if (response.data == null) {
                    this.open("Đặt xe không thành công");
                }
                if (response.data.success) {
                    this.open("Đặt xe thành công");
                    console.log("Đặt xe thành công");
                    this.$router.push({ name: 'user-profile-historyBooking' });
                }
            }
        },

        open(message) {
            this.$refs.message.ShowMess(message);
        },

        // Voucher khi đã chọn
        async UserVoucher(id, bien) {
            await this.fetchPromotions(id);
            this.userPromotion = bien;
            this.booking.promotionId = id;
            this.booking.discountValue = this.promotion.discountValue;
            this.ToTal();
            if (bien == 2) {
                this.userKM = false;
                this.adminKM = true;
            }
            else {
                this.adminKM = false;
                this.userKM = true;
            }
        },
        ShowVouCherPost() {
            console.log("post từ create : ", this.postId);
            this.$emit('ShowVouCher', this.postId, this.booking.total);
        },
        ShowVouCherAdmin() {
            this.$emit('ShowVouCher', -1, this.booking.total);
        },
        // Nhận được ngày nhận và ngày trả

        async getDayHour(start, end) {
            this.booking.recieveOn = start;
            this.booking.returnOn = end;
            this.boolHour = true;
            const startDate = new Date(start);
            const endDate = new Date(end);
            // Tính chênh lệch thời gian giữa hai ngày (theo milliseconds)
            const differenceInMilliseconds = endDate - startDate;
            console.log("Diff: ", differenceInMilliseconds);
            // Chuyển đổi từ milliseconds sang giờ
            this.sumhour = differenceInMilliseconds / (1000 * 60 * 60);
            console.log("Sum hour: ", this.sumhour);
            this.ToTal();
        },
        ClickDay() {
            this.hour = !this.hour;
            this.$emit('requestAction', this.hour);
        },
        isDayMoreThan3(postDay) {
            // Chuyển ngày truyền vào và ngày hiện tại thành đối tượng Date
            const postDate = new Date(postDay);
            const currentDate = new Date();

            // Đặt thời gian của ngày hiện tại về đầu ngày (00:00:00) để so sánh chính xác
            currentDate.setHours(0, 0, 0, 0);
            postDate.setHours(0, 0, 0, 0);

            // Tính số ngày chênh lệch
            const diffTime = (postDate - currentDate); // Kết quả là mili giây
            const diffDays = diffTime / (1000 * 60 * 60 * 24); // Chuyển sang ngày

            console.log("Ngày truyền vào: ", postDay);
            console.log("Số ngày chênh lệch: ", diffDays);

            // Trả về true nếu số ngày chênh lệch lớn hơn 3
            return diffDays >= 3;
        },

        // lấy bài viết
        async fetchPost() {
            const id = this.$route.params.id;
            //console.log("Id của bài post: ", id);
            try {
                const response = await axios.get(`http://localhost:5027/api/User/Post/GetById/${id}`)
                this.post = response.data.data;
            }
            catch (error) {
                console.log(error);
            }
        },
        //lấy khuyến mãi
        async fetchPromotions(id) {
            try {
                const response = await PromotionService.getPromotion(id);
                if (response && response.data) {
                    this.promotion = response.data;
                } else {
                    console.warn("No promotion data received");
                }
            } catch (error) {
                console.error("Error fetching promotion:", error);
                throw error; // Re-throw để có thể bắt ở UserVoucher
            }
        },
        XoaKM() {
            this.promotion = null;
            this.userPromotion = 0;
            this.userKMAd = false;
            this.userKM = false;
            this.adminKM = false;
            this.booking.promotionId = null;
            this.booking.discountValue = 0;
            this.ToTal();
        },
        format(price) {
            if (price > 1) {
                return price / 1000;
            }
        },
        formatMoney(value) {
            return new Intl.NumberFormat('vi-VN').format(value);
        },
        formatPrice(price) {
            if (price >= 1000000) {
                return (price / 1000000).toFixed(1).replace('.0', '') + "Tr";
            } else if (price >= 1000) {
                return (price / 1000).toLocaleString('en').replace(/,/g, '.') + "k";
            } else {
                return price.toString();
            }
        },
        async Submit(lat, lng, address) {
            this.booking.latitude = lat;
            this.booking.longitude = lng;
            this.showMap = false;
            this.address = address;
            console.log("Address: ", address, "Lat: ", lat, "Lng: ", lng);
        },
        Close() {
            this.showMap = false;
        },
    },
    async created() {
        await this.fetchPost();
        this.booking.postId = this.post.id;
        console.log("Post Create: ", this.post);
        const userString = sessionStorage.getItem('User');
        if (userString != null) {
            this.authen = true;
        }

    },

}
</script>

<style>
.z {
    display: flex;
    justify-content: center;
    align-content: center;
    align-items: center;
    justify-items: center;
}

/* .listvoucher{
    position: absolute;
    width: 100%;
    height: 500px;
} */
.priceBooking {
    display: flex;
    justify-content: space-between;
}

.sale {
    padding-top: 5px;
    justify-content: space-between;
    display: flex;
    align-items: center;
    color: #434343;
    font-size: .9rem;
    font-weight: 500;
    line-height: 16px;
}

.x {
    color: #434343;
}

.x:hover {
    color: #000;
}

input[type="checkbox"] {
    width: 20px;
    height: 20px;
    cursor: pointer;
    appearance: none;
    /* Xóa mặc định */
    border: 2px solid var(--color-green2);
    background-color: transparent;
    border-radius: 4px;
    /* Làm góc bo tròn nếu cần */
    position: relative;
}

input[type="checkbox"]:checked {
    background-color: var(--color-green2);
    /* Màu nền khi được chọn */
    /* Đường viền */
}

input[type="checkbox"]:checked::before {
    content: "✔";
    /* Biểu tượng check */
    font-size: 14px;
    color: rgb(255, 255, 255);
    /* Màu biểu tượng check */
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
}




.khuyenmai {
    width: 100%;
    height: 50px;
    display: flex;
    align-items: center;
}

.khuyenmai_trai {
    display: flex;
    padding: 10px 0;
    width: 20%;
}

.khuyenmai_trai img {
    margin-left: 5px;
    margin-top: -5px;
    height: 35px !important;
}

.khuyenmai_phai {
    padding: 10px;
    width: 80%;
}
</style>
