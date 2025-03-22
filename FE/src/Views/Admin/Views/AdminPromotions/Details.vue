<template>
  
<h1>Details</h1>

<div>
    <h4>Promotion</h4>
    <hr />
    <dl class="row">
        <dt class = "col-sm-2">
            Nội dung
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.content }}
        </dd>
        <dt class = "col-sm-2">
            Giá trị khuyến mãi
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.discountValue }}
        </dd>
        <dt class = "col-sm-2">
            Ngày kết thúc
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.expiredDate }}
        </dd>
        <dt class = "col-sm-2">
            Mã người tạo
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.createdById }}
        </dd>
        <dt class = "col-sm-2">
            Ngày tạo
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.createdOn }}
        </dd>
        <dt class = "col-sm-2">
            Mã người cập nhật
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.modifiedById }}
        </dd>
        <dt class = "col-sm-2">
            Ngày cập nhật
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.modifiedOn }}
        </dd>
        <dt class = "col-sm-2">
            Ẩn
        </dt>
        <dd class = "col-sm-10">
            {{ promotion.isDeleted }}
        </dd>
    </dl>
</div>
<div>
    <a asp-action="Edit" asp-route-id="@Model?.Id">Edit</a> |
    <a href="javascript:history.go(-1);">Quay lại</a>
</div>

</template>

<script>
import PromotionService from '../../../../Service/api/PromotionService';
import PromotionVM from '../../../../Model/PromotionVM';
export default {
    data(){
        return {
            promotion: new PromotionVM(),
        }
    },
    methods:{
        async getPromotion(){
            const id = this.$route.params.id;
           
            try{
                const response = await PromotionService.getPromotion(id);
                this.promotion = response.data;
            }
            catch(error){
                console.log(error);
            }
        }
    },
created(){
    this.getPromotion();
},
}
</script>

<style>

</style>
