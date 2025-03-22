<template>

    <h1>Edit</h1>

    <h4>Promotion</h4>
    <hr />
    <div class="row">
        <div class="col-md-4">
            <!-- <form asp-action="Edit">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group">
                <label asp-for="Content" class="control-label"></label>
                <input asp-for="Content" class="form-control" />
                <span asp-validation-for="Content" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="DiscountValue" class="control-label"></label>
                <input asp-for="DiscountValue" class="form-control" />
                <span asp-validation-for="DiscountValue" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="ExpiredDate" class="control-label"></label>
                <input asp-for="ExpiredDate" class="form-control"/>
                <span asp-validation-for="ExpiredDate" class="text-danger"></span>
            </div>
            Trạng thái
            <div class="form-group form-check">
                <label class="form-check-label">
                    <input class="form-check-input" asp-for="IsDeleted" /> Ẩn
                </label>
            </div>
            <div class="form-group">
                <input type="submit" value="Save" class="btn btn-primary" />
            </div>
        </form> -->
            <form @submit.prevent="submitForm" enctype="multipart/form-data">
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                <div class="form-group">
                    <label for="Content" class="control-label">Nội dung</label>
                    <input v-model="promotion.content" type="text" class="form-control" />
                    <span for="Content" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label for="DiscountValue" class="control-label">Giá trị khuyến mãi</label>
                    <input v-model.number="promotion.discountValue" type="number" step="0.01" min="0.01" max="1"
                        class="form-control" />
                    <span for="DiscountValue" class="text-danger"></span>
                </div>

                <div class="form-group">
                    <label for="ExpiredDate" class="control-label">Ngày kết thúc</label>
                    <input v-model="promotion.expiredDate" type="datetime-local" class="form-control" required />
                    <span for="ExpiredDate" class="text-danger"></span>
                </div>
                <div class="form-group">
                    <label class="control-label" for="Isdeleted">Trạng thái</label>
                    <label class="form-check-label">
                        <input class="form-check-input" type="checkbox" :checked="promotion.isDeleted" /> Ẩn
                    </label>
                </div>
                <div class="form-group">
                    <input type="submit" value="Tạo" class="btn btn-primary" />
                </div>
            </form>
        </div>
    </div>

    <div>
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>
</template>

<script>
import PromotionService from '../../../../Service/api/PromotionService';
import PromotionVM from '../../../../Model/PromotionVM';
import PromotionDTO from '../../../../DTOs/PromotionDto';
export default {
    data() {
        return {
            promotion: new PromotionVM(),
            editPromotion: new PromotionDTO()
        }
    },
    methods: {
        async getPromotion() {
            const id = this.$route.params.id;

            try {
                const response = await PromotionService.getPromotion(id);
                this.promotion = response.data;
            }
            catch (error) {
                console.log(error);
            }
        },
        async submitForm() {
            if (this.promotion.content == "" || this.promotion.discountValue <= 0 || this.promotion.expiredDate == null) {
                alert('Vui lòng điền đủ thông tin.');
                return;
            }
            const date = new Date(this.promotion.expiredDate);
            const formattedDate = date.toISOString().slice(0, 16);

            this.promotion.expiredDate = formattedDate;
            this.editPromotion = {
                id: this.promotion.id,
                content: this.promotion.content,
                discountValue: this.promotion.discountValue,
                expiredDate: this.promotion.expiredDate
            };
            try {
                const result = await PromotionService.UpdatePromotion(this.editPromotion);
                console.log(result);
                this.$router.push({ name: 'admin-promotion' });
            }
            catch (error) {
                console.log(error);
            }
        }
    },
    created() {
        this.getPromotion();
    },
}
</script>


<style></style>
