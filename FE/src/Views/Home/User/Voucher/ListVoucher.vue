<template>

    <div style="position: relative;">

        <div class="listvoucher">
            <span class="icon-closeForm" style="position: absolute;top: 0;right: 0;">
                    <a class="custom-link" @click="close()">
                        <i class="ri-close-line"></i>
                    </a>
                </span>
            <div class="modal-content">
                
                <div class="modal-header">

                    <h5>Mã khuyến mãi</h5>
                </div>
                <div class="modal-body">
                    <div class="add-promotion">
                        <div v-if="promotion && promotion.length > 0 && rolePromotion == 1" style="margin-top: 15px;">
                            <div v-for="item in promotion" :key="item">
                                <div class="add-promotion__item" style="position: relative;"
                                    v-if="item.promotion.discountValue > 1 && item.promotion.discountValue <= total || item.promotion.discountValue < 1">
                                    <div class="promotion-info" v-if="item.promotion.discountValue < 1">
                                        <p class="name">{{ item.promotion.content }}</p>
                                        <p class="detail">Giảm {{ item.promotion.discountValue * 100 }} %. <span
                                                @click="ShowDetail()">Chi tiết</span></p>
                                        <div hidden style="width: 80%;">Mã khuyến mãi được tạo bởi chủ xe, và có giá trị
                                            giảm {{ item.promotion.discountValue * 100 }}%. Có hiệu lực đến ngày {{
                                                format(item.promotion.expiredDate) }}.</div>
                                    </div>
                                    <div class="promotion-info" v-else>
                                        <p class="name">{{ item.promotion.content }}</p>
                                        <p class="detail">Giảm {{ formatPrice(item.promotion.discountValue) }}VND. <span
                                                @click="ShowDetail()">Chi tiết</span></p>
                                        <div hidden style="width: 80%;">Mã khuyến mãi được tạo bởi chủ xe, và có giá trị
                                            giảm {{ formatPrice(item.promotion.discountValue) }}VND cho đơn đặt xe tối thiểu {{
                                                item.promotion.discountValue + 100000 }}. Có hiệu lực đến ngày {{
                                                format(item.promotion.expiredDate) }}.</div>
                                    </div>
                                    <a class="btn btn-primary" @click="UseVoucher(item.promotion.id, 1)">Áp dụng</a>
                                </div>

                            </div>
                        </div>
                        <div v-if="promotion && promotion.length > 0 && rolePromotion == 2" style="margin-top: 15px;">
                            <!-- {{ promotion }} -->
                            <div v-for="item in promotion" :key="item">
                                <div class="add-promotion__item" style="position: relative;" >
                                    <!-- v-if="item.discountValue <= total && new Date(item.expiredDate).getTime() < Date.now()" -->
                                    <div class="promotion-info" v-if="item.discountValue < 1">
                                        <p class="name">{{ item.content }}</p>
                                        <p class="detail">Giảm {{ item.discountValue * 100 }} %. <span
                                                @click="ShowDetail()">Chi tiết</span></p>
                                        <div hidden style="width: 80%;">Mã khuyến mãi được tạo bởi chủ xe, và có giá trị
                                            giảm {{ item.discountValue * 100 }}%. Có hiệu lực đến ngày {{
                                                format(item.expiredDate) }}.</div>
                                    </div>
                                    <div class="promotion-info" v-else>
                                        <p class="name">{{ item.content }}</p>
                                        <p class="detail">Giảm {{ formatPrice(item.discountValue) }}VND. <span
                                                @click="ShowDetail()">Chi tiết</span></p>
                                        <div hidden style="width: 80%;">Mã khuyến mãi được tạo bởi chủ xe, và có giá trị
                                            giảm {{ formatPrice(item.discountValue) }} VND cho đơn đặt xe tối thiểu từ {{
                                                item.discountValue + 100000 }}. Có hiệu lực đến ngày {{
                                                format(item.expiredDate) }}.</div>
                                    </div>
                                    <a class="btn btn-primary" @click="UseVoucher(item.id, 2)">Áp dụng</a>
                                </div>


                            </div>
                        </div>
                        <div v-if="promotion && promotion.length == 0">
                            <div class="add-promotion__item">
                                Không có khuyến mãi
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
                                <!-- <div class="wrap-svg"></div>
                                <div class="promotion-info">
                                    <p class="name">MI75</p>
                                    <p>Giảm 8% (tối đa 80K). <span>Chi tiết</span></p>
                                </div>
                                <a class="btn btn-primary">Áp dụng</a> -->
</template>

<script>
import PostService from '../../../../Service/api/PostService';
import PromotionService from '../../../../Service/api/PromotionService';
import PostPromotionService from '../../../../Service/api/PostPromotionService';
import PromotionVM from '../../../../Model/PromotionVM';
export default {
    data() {
        return {
            promotion: [],
            rolePromotion: 0,
            total: 0
        };
    },
    methods: {
        close() {
            this.$emit('Close');
        },
        UseVoucher(id, bien) {
            if (id != 0) {
                this.$emit('UserVoucher', id, bien);
            }
        },
        async ShowList(id, tot) {
            this.total = tot;
            console.log("IP của bài post: ",id);
            console.log("total:", this.total);
            if (id >= 0) {
                this.rolePromotion = 1;
                const response = await PostService.getPost(id);
                this.promotion = response.data.postPromotions;
                console.log('Post voucher', this.promotion);

            }
            else {
                this.rolePromotion = 2;
                const response = await PromotionService.getAllByUserPromotion();
                console.log('Post voucher by admin', response);
                this.promotion = response.data;
                console.log(this.promotion);
            }
        },
        format(dateString) {
            const date = new Date(dateString);

            // Lấy giờ và phút, đảm bảo định dạng 2 chữ số
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');

            // Lấy ngày, tháng, và năm, đảm bảo định dạng 2 chữ số
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();

            // Kết hợp các phần lại theo định dạng yêu cầu
            return `${hours}:${minutes} ${day}-${month}-${year}`;
        },
        formatPrice(value) {
            return new Intl.NumberFormat('vi-VN').format(value);
        },
    },
    created() {

    },

}

</script>

<style>
.modal-content .modal-header .close {
    border-radius: 100%;
    border: 1px solid #e0e0e0;
    position: absolute;
    font-size: 1.25rem;
    font-weight: 800;
    line-height: .5;
    color: #000;
    right: 0;
    top: 0;
    padding: 10px;
    z-index: 1;
}

.modal-dialog.modal-fix .modal-content .modal-header h5 {
    position: absolute;
    top: 0;
    left: 50%;
    transform: translateX(-50%);
    width: 100%;
}

.add-promotion__item {
    display: flex;
    grid-gap: 8px;
    gap: 8px;
    /* height: 50px; */
    padding: 10px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    border-radius: 10px;
    align-items: center;
    margin-bottom: 10px;
}

.add-promotion__item .promotion-info .detail {
    font-size: .75rem;
    font-weight: 500;
}

p {
    padding: 0;
    margin: 0;
}

.add-promotion__item .btn {
    width: 90px;
    height: 40px;
    margin: 0 0 0 auto;
    height: -webkit-min-content;
    height: min-content;
}

h5 {
    font-size: 1.5rem;
    font-weight: 700;
}

.modal-dialog.modal-fix .modal-content .modal-body {
    max-height: calc(100dvh - 145px);
    overflow-x: hidden;
    overflow-y: auto;

}

.listvoucher {
    max-height: 700px;
    padding: 24px;
    height: 700px;
    width: 500px;
    background-color: aliceblue;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
    border-radius: 15px;
}
.an{
    opacity: 0;
}
</style>