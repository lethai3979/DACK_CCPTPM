<template>
    <div style="margin-bottom:20px;">
        <button class="btn nutshort" style="width: 200px;" @click="QuayLai()">Quay lại</button>
    </div>

    <h1>Thêm mới khuyến mãi</h1>
    <hr />
    <div class="row">
        <div class="">
            <form @submit.prevent="submitForm" enctype="multipart/form-data">
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                <div class="form-group">
                    <label for="Content" class="control-label">Nội dung</label>
                    <input v-model="promotion.content" type="text" class="form-control" />
                    <span for="Content" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label for="DiscountValue" class="control-label">Giá trị khuyến mãi</label>
                    <input v-model.number="promotion.discountValue" type="number" step="0.01" min="0.01"
                        class="form-control" />
                    <span for="DiscountValue" class="text-danger"></span>
                </div>

                <div class="form-group">
                    <label for="ExpiredDate" class="control-label">Ngày kết thúc</label>
                    <input v-model="promotion.expiredDate" type="datetime-local" class="form-control" required />
                    <span for="ExpiredDate" class="text-danger"></span>
                </div>



                <div class="form-group">
                    <label for="ExpiredDate" class="control-label">Các bài đăng được sử dụng</label>
                    <ul v-if="post != null">
                        <li v-for="item in post" :key="item.id">
                            <div class="form-check" v-if="item.isDisabled !== true">
                                <input class="form-check-input car-type-checkbox" type="checkbox" :value="item.id"
                                    v-model="promotion.postIds">
                                <label class="form-check-label">
                                    {{ item.name }}
                                </label>
                            </div>
                        </li>
                    </ul>
                    <ul v-else>
                        Chưa có bài đăng nào.
                    </ul>
                    <span for="ExpiredDate" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <input type="submit" value="Tạo" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>

</template>

<script>
import PromotionDTO from '../../../../DTOs/PromotionDto';
import PostVM from '../../../../Model/PostVM';
import PostPromotionService from '../../../../Service/api/PostPromotionService';
import PostService from '../../../../Service/api/PostService';

export default {
  created () {
    this.getAllPosst();
  },
    data() {
        return {
            promotion: new PromotionDTO(),
            post: [new PostVM()]
        };
    },
    methods: {
        async getAllPosst(){
            try{
                const response = await PostService.getAllPostByUserId();
                console.log("response",response.data);
                if(response.success){
                    this.post = response.data;
                }
            }
            catch(error){
                console.log("Có lỗi: ", error);
            }
        },
        QuayLai(){
            this.$emit('QuayLai',0);
        },
        
        async submitForm() {
            console.log(this.promotion);

            if (this.promotion.content == "" || this.promotion.discountValue <= 0 || this.promotion.expiredDate == null) {
                alert('Vui lòng điền đủ thông tin.');
                return;
            }
            this.promotion.expiredDate = new Date(this.promotion.expiredDate).toISOString();
            console.log(this.promotion);
            try {
                const response = await PostPromotionService.AddPromotion(this.promotion);
                console.log("Response: ",response);
                    if(response.success){
                        console.log('Dữ liệu đã được gửi thành công:', response);
                        this.QuayLai();
                    }
                    
                
            } catch (error) {
                console.error('Lỗi kết nối:', error);
                alert('Lỗi kết nối hoặc lỗi khác. Vui lòng thử lại sau.');
            }
        }

    }
}
</script>

<style></style>
