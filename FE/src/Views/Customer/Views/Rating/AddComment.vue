<template>
  <!-- <h2>Thêm bình luận:</h2> -->
  <div class="" style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);padding: 20px 30px;border-radius: 15px;">
    <div class="">
      <form @submit.prevent="submitComment" enctype="multipart/form-data">
        <div class="form-group">
          <label class="control-label">Bình luận</label>
          <input v-model="ratingDto.comment" class="form-control" required />
        </div>
        <div class="form-group">
          <label class="control-label">Đánh giá</label>
          <ul style="display: flex;">
            <li v-for="item in 5" :key="item">
              <button type="button" @click="pick(item)" class="star-button">
                <!-- {{ item }}   -->
                <img v-if="stars[item] === 0" width="26" height="26" src="https://img.icons8.com/metro/26/star.png"
                  alt="star" />
                <img v-else width="26" height="26" src="https://img.icons8.com/fluency/48/star--v1.png"
                  alt="star--v1" />
              </button>
            </li>
          </ul>
        </div>
        <div class="form-group">
          <input type="submit" value="Bình luận" class="btn btn-primary" />
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import RatingDTO from '../../../../DTOs/RatingDto';

export default {
  props: {
    id: {
      type: Number,
      default: 0,
      required: true
    },
  },
  data() {
    return {
      componentKey:0,
      stars: {
        1: 0,
        2: 0,
        3: 0,
        4: 0,
        5: 0,
      },
      ratingDto: new RatingDTO(),
      selectedStars: 0, // Số sao người dùng đã chọn
    };
  },
  methods: {
    // Hàm chọn sao
    pick(selectedStar) {
      this.ratingDto.point = selectedStar;
      for (let i = 1; i <= 5; i++) {
        this.stars[i] = i <= selectedStar ? 1 : 0; // Gán sao nhỏ hơn hoặc bằng selectedStar là 1, ngược lại là 0
      }
    },
    // Hàm submit bình luận
    async submitComment() {
      if (!this.ratingDto.comment) {
        alert("Vui lòng nhập nội dung bình luận");
        return;
      }
      if (this.ratingDto.point === 0) {
        alert("Vui lòng chọn số sao đánh giá");
        return;
      }
      this.ratingDto.postId = this.id;
      const formData = new RatingDTO(this.ratingDto).toFormData();
      const jsonData = {};
      formData.forEach((value, key) => {
        jsonData[key] = value;
      });      
      // const jsonData = JSON.stringify(formData);
      console.log("Data gửi đi : ", jsonData);
      const token = sessionStorage.getItem("authToken");
      const response = await axios.post(
        "http://localhost:5027/api/User/RatingAndComment/Add", JSON.stringify(jsonData),
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      console.log("Response trả về add comment: ",response);
      if(response.status == 200){
        if(response.data.success){
          this.ratingDto = new RatingDTO(); 
          this.stars = [0, 0, 0, 0, 0, 0];
          this.$emit('GetAllRatings');
        }
      }

      // Gửi dữ liệu bình luận và sao đánh giá tới server
      // Bạn có thể thực hiện API call ở đây để submit form
    },
  },
};
</script>

<style scoped>
/* Thêm chút styling để nút sao đẹp hơn */
.star-button {
  border: none;
  background: none;
  cursor: pointer;
}

.star-button img {
  vertical-align: middle;
}
</style>






<!-- @model DoAnCNTT.Models.Rating

@{
    ViewData["Title"] = "AddComment";
    //    Layout = "~/Views/Shared/_Layout.cshtml";
}
<h2>Thêm bình luận:</h2>
<div class="row">
    <div class="col-md-4">
        <form asp-action="AddComment" asp-controller="Rating" enctype="multipart/form-data">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group">
                <label asp-for="Comment" class="control-label">Bình luận</label>
                <input asp-for="Comment" class="form-control" />
                <span asp-validation-for="Comment" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Point" class="control-label">Đánh giá</label>
                <div style="display:flex;">
                    <div class="form-check">
                        <input asp-for="Point" class="form-check-input" type="radio" id="point1" value="1">
                        <label class="form-check-label" for="point1">1 sao</label>
                    </div>
                    <div class="form-check">
                        <input asp-for="Point" class="form-check-input" type="radio" id="point2" value="2">
                        <label class="form-check-label" for="point2">2 sao</label>
                    </div>
                    <div class="form-check">
                        <input asp-for="Point" class="form-check-input" type="radio" id="point3" value="3">
                        <label class="form-check-label" for="point3">3 sao</label>
                    </div>
                    <div class="form-check">
                        <input asp-for="Point" class="form-check-input" type="radio" id="point4" value="4">
                        <label class="form-check-label" for="point4">4 sao</label>
                    </div>
                    <div class="form-check">
                        <input asp-for="Point" class="form-check-input" type="radio" id="point5" value="5">
                        <label class="form-check-label" for="point5">5 sao</label>
                    </div>
                </div>
                <span asp-validation-for="Point" class="text-danger"></span>
            </div>
            <div class="form-group" hidden>
                <label asp-for="UserId" class="control-label"></label>
                <input asp-for="UserId" class="form-control"></input>
            </div>
            <div class="form-group" hidden>
                <label asp-for="PostId" class="control-label"></label>
                <input asp-for="PostId" class="form-control"></input>
            </div>
            <div class="form-group" hidden>
                <label asp-for="CreatedById" class="control-label"></label>
                <input asp-for="CreatedById" class="form-control" />
                <span asp-validation-for="CreatedById" class="text-danger"></span>
            </div>
            <div class="form-group" hidden>
                <label asp-for="CreatedOn" class="control-label"></label>
                <input asp-for="CreatedOn" class="form-control" type="datetime" />
                <span asp-validation-for="CreatedOn" class="text-danger"></span>
            </div>
            <div class="form-group" hidden>
                <label asp-for="ModifiedById" class="control-label"></label>
                <input asp-for="ModifiedById" class="form-control" />
                <span asp-validation-for="ModifiedById" class="text-danger"></span>
            </div>
            <div class="form-group" hidden>
                <label asp-for="ModifiedOn" class="control-label"></label>
                <input asp-for="ModifiedOn" class="form-control" />
                <span asp-validation-for="ModifiedOn" class="text-danger"></span>
            </div>
            <div class="form-group form-check" hidden>
                <label class="form-check-label">
                    <input class="form-check-input" asp-for="IsDeleted" /> @Html.DisplayNameFor(model => model.IsDeleted)
                </label>
            </div>
            <div class="form-group">
                <input type="submit" value="Bình luận" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>


@section Scripts {
    @{
        await Html.RenderPartialAsync("_ValidationScriptsPartial");
    }
} -->
