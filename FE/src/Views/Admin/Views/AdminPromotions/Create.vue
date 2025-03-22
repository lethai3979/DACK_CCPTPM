<template>
    <div style="margin-bottom:20px;">
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

    <h1>Thêm mới khuyến mãi</h1>
    <hr />
    <div >
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
                    <input type="submit" value="Tạo" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>

</template>

<script>
import PromotionDTO from '../../../../DTOs/PromotionDto';
import PromotionService from '../../../../Service/api/PromotionService';

export default {
    data() {
        return {
            promotion: new PromotionDTO(),

        };
    },
    methods: {
        async submitForm() {
            console.log(this.promotion);

            if (this.promotion.content == "" || this.promotion.discountValue <= 0 || this.promotion.expiredDate == null) {
                alert('Vui lòng điền đủ thông tin.');
                return;
            }
            this.promotion.expiredDate = new Date(this.promotion.expiredDate).toISOString();

            try {
                const response = await PromotionService.AddPromotion(this.promotion);
                
                    console.log('Dữ liệu đã được gửi thành công:', response);
                    this.$router.push({ name: 'admin-promotion' });
                
            } catch (error) {
                console.error('Lỗi kết nối:', error);
                alert('Lỗi kết nối hoặc lỗi khác. Vui lòng thử lại sau.');
            }
        }

    }
}
</script>

<style></style>
