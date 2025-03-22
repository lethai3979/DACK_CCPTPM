<template>
    <div v-if="isdelete == true" style="justify-content: center;justify-items: center;background-color: aqua;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);border-radius: 15px;padding: 20px 30px;width: 70%;position: absolute;margin-top: 150px;">
            <h3>Bạn có chắc chắn muốn xóa</h3>
            <div>
                <button @click="Submit()" class="btn nutshort" style="margin-right: 20px;">Xóa</button>
                <button @click="isdelete = false" class="btn1 nutshort">Không</button>
            </div>
        </div>
    <div>
        <div v-if="page == 0">
            <h5>Danh sách khuyến mãi xe của bạn</h5>
            <button @click="page = 1" class="btn nutshort">Tạo khuyến mãi </button>
            <ul v-if="promotions != null">
                <div class="voucherlist" v-for="item in promotions" :key="item">
                    <div class="voucher1">
                        <div class="gift">
                            <li>GIFT</li>
                            <li>VOU</li>
                            <li>CHER</li>
                        </div>

                        <!-- {{ item }} -->
                        <div>
                            <label style="margin-right: 20px;">{{ item.content }}</label>
                            <label v-if="new Date(item.expiredDate).getTime() > Date.now()" for="">Thời gian hết hạn: {{ format(item.expiredDate) }}</label>
                            <label v-else for="">Đã hết hạn, hãy gia hạn hoặc tạo mới</label>
                        </div>
                        <div>
                            <button @click="page = 2;bien = item.id" class="buttonDelete"><img width="24" height="24"
                                    src="https://img.icons8.com/fluency-systems-filled/50/edit.png"
                                    alt="edit" /></button>
                            <button @click="isdelete = true;idDelete = item.id " class="buttonDelete"><img width="24" height="24"
                                    src="https://img.icons8.com/material-rounded/24/delete-forever.png"
                                    alt="delete-forever" /></button>
                        </div>
                    </div>

                    <!-- </div> -->

                </div>
            </ul>
            <ul v-else>
                <li>Danh sách khuyến mãi trống</li>
            </ul>
        </div>


        



        <div v-if="page == 1">
            <Create @QuayLai="QuayLai" />
        </div>
        <div v-if="page == 2" :Id="heh">
            <Edit :Id="bien" @Close="Close"/>
        </div>
        <div v-if="page == 3">
            <Use />
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import Create from './Voucher/CreateVoucher.vue';
import Edit from './Voucher/EditVoucher.vue';
import Use from './Voucher/UseVoucher.vue';
import PostPromotionService from '../../../Service/api/PostPromotionService';
import PromotionVM from '../../../Model/PromotionVM';
export default {
    props: {
    },
    components: {
        Create, Edit, Use
    },
    data() {
        return {
            page: 0,
            promotions: [new PromotionVM()],
            bien:0,
            isdelete: false,
            idDelete:0
        }
    },
    methods: {
        async Submit(){
            const response = await PostPromotionService.DeletePromotion(this.idDelete);
            console.log(response);
            if(response.success){
                this.isdelete = false;
                await this.getAllPromotion();
            }
            else{
                console.log("lỗi");
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
        QuayLai(page) {
            this.page = page;
            this.getAllPromotion();
        },
        async getAllPromotion() {
            const token = sessionStorage.getItem("authToken");
            try {
                const response = await PostPromotionService.getAllPromotion();
                this.promotions = response.data;
                console.log(response.data);
            }
            catch (error) {
                console.log(error);
            }
        },
        Close(){
            this.page = 0;
        }

    },
    created() {
        this.getAllPromotion();
    },

}
</script>

<style>
.voucherlist {
    width: 100%;
    margin-top: 20px;
    margin-bottom: 20px;
}

.voucherlist .voucher1 {
    justify-content: space-between;
    padding: 20px 30px;
    display: flex;
    border: 1px solid;
    width: 95%;
    border-radius: 5px;

}

.voucherlist .voucher1 div {
    justify-content: center;
    justify-items: center;
    align-items: center;
    align-content: center;
}

.voucher .gift {
    display: block;
}
</style>