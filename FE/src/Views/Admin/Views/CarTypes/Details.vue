<template>
  
<h1>Chi tiết</h1>

<div>
    <h4>Loại xe</h4>
    <hr />
    <dl class="row" v-if="cartype!=null">
        <dt class = "col-sm-2">
            Tên
        </dt>
        <dd class = "col-sm-10">
            {{ cartype.name }}
        </dd>
        <dt class="col-sm-2">
            Tên hãng xe:
        </dt>
        <dd class="col-sm-10" v-for="item in cartype.carTypeDetail" :key="item.id">
            
                    <div>{{ item.companyName }}</div>
               
        </dd>
        <dt class = "col-sm-2">
            Mã người tạo
        </dt>
        <dd class = "col-sm-10">
            {{ cartype.createdById }}
        </dd>
        <dt class = "col-sm-2">
            Ngày tạo
        </dt>
        <dd class = "col-sm-10">
            {{ cartype.createdOn }}
        </dd>
        <dt class = "col-sm-2">
            Mã người cập nhật
        </dt>
        <dd class = "col-sm-10">
            {{ cartype.modifiedById }}
        </dd>
        <dt class = "col-sm-2">
            Ngày cập nhật
        </dt>
        <dd class = "col-sm-10">
            {{ cartype.modifiedOn }}
        </dd>
        <dt class = "col-sm-2">
            Ẩn
        </dt>
        <dd class = "col-sm-10">
            {{ cartype.isDeleted }}
        </dd>
    </dl>
</div>
<div>
    <a asp-action="Edit" asp-route-id="@Model?.Id">Chỉnh sửa</a> |
    <a href="javascript:history.go(-1);">Quay lại</a>
</div>

</template>
<script>
import CarTypeService from '../../../../Service/api/CarTypeService';
import CarTypeVM from '../../../../Model/CartypeVM';
export default {
    data() {
        return {
            cartype: new CarTypeVM(),
        };
    },
    created() {
        this.getCartypeById();
    },
    methods: {
        async getCartypeById() {
            const id = this.$route.params.id;
            try {
                const res_cartype = await CarTypeService.getCartypeById(id);
                console.log("Cartype: ",res_cartype.data);
                this.cartype = res_cartype.data;
            }
            catch (error) {
                console.error('Lỗi lấy dữ liệu:', error);
            }
        }
    },
}
</script>

<style>

</style>

