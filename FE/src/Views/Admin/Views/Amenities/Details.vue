<template>

    <h1>Chi tiết tiện nghi</h1>

    <div>
        <h4>Thông tin tiện nghi</h4>
        <hr />
        <dl class="row" v-if="amenity != null">
            <dt class="col-sm-2">
                Tên
            </dt>
            <dd class="col-sm-10">
                {{ amenity.name }}
            </dd>
            <dt class="col-sm-2">
                Icon
            </dt>
            <dd class="col-sm-10">
                <img :src="amenity.iconImage" alt="hinhanh.jpg" width="200px" />
            </dd>
             <dt class="col-sm-2">
                Mã người tạo
            </dt>
            <dd class="col-sm-10">
                {{ amenity.createdById }}
            </dd>
            <dt class="col-sm-2">
                Ngày tạo
            </dt>
            <dd class="col-sm-10">
                {{ amenity.createdOn }}
            </dd>
            <dt class="col-sm-2">
                Mã người cập nhật
            </dt>
            <dd class="col-sm-10">
                {{ amenity.modifiedById }}
            </dd>
            <dt class="col-sm-2">
                Ngày cập nhật
            </dt>
            <dd class="col-sm-10">
                {{ amenity.modifiedOn }}
            </dd>
            <dt class="col-sm-2">
                Ẩn
            </dt>
            <dd class="col-sm-10">
                {{ amenity.isDeleted }}
            </dd> 
        </dl>
        <h1 v-else>
            Loading.......
        </h1>
    </div>
    <div>
        <!-- <a asp-action="Edit" asp-route-id="@Model?.Id">Chỉnh sửa</a> | -->
        <a href="javascript:history.go(-1);">Quay lại</a>
    </div>

</template>

<script>
import AmenityService from '../../../../Service/api/AmenityService';
import Amenity from '../../../../Model/AmenityVM';
import axios from 'axios';
export default {

    data() {
        return {
            amenity: new Amenity(),
        }
    },
    methods: {
        async getAmenityById() {
            const id = this.$route.params.id;
            try {
                const result = await AmenityService.getAmenityById(id);
                this.amenity = result.data;
                console.log('Dữ liệu đã được lấy thành công:', result.data);
            } catch (error) {
                alert('Có lỗi xảy ra: ' + error.message);
            }
        }
    },
    // mounted() {
    //     this.getAmenityById();
    // },   
    created() {
        this.getAmenityById();
    }
}
</script>

<style></style>
