<template>
    <h1>Lịch sử đặt xe của bạn</h1>

    <div>
        Quy trình thuê xe
        <ul class="ulview">
            <li>Bước 1: <i style="color: orange;">Chờ xác nhận</i></li>
            <li>Bước 2: <i style="color: orange;">Vui lòng thanh toán</i></li>
            <li>Bước 3: <i style="color: orange;">Chờ nhận xe</i></li>
            <li>Bước 4: <i style="color: rgb(0, 102, 254);">Đang thuê</i></li>
            <li>Bước 5: <i style="color: green;">Hoàn thành</i></li>
        </ul>

    </div>

    <div v-for="item in booking" :key="item.id">


        <!-- var currentUser = await UserManager.FindByIdAsync(item.Post.UserId!); -->
        <div
            style="box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);margin:10px;padding:30px;margin-bottom:20px;background-color:white;border-radius:5px;">

            <div class="tren">
                <div class="trai">Tên chủ xe : {{ item.post.name }}</div>
                <div class="phai" style="font-size:20px;">
                    <label v-if="item.status == 'Pending'" style="color: orange;">
                        Chờ xác nhận
                    </label>
                    <label v-if="item.status == 'Accept Booking'" style="color: orange;">
                        Vui lòng thanh toán
                    </label>
                    <label v-if="item.status == 'Waiting'" style="color: orange;">
                        Chờ nhận xe
                    </label>
                    <label v-if="item.status == 'Renting'" style="color: rgb(0, 102, 254);">
                        Đang thuê
                    </label>
                    <label v-if="item.status == 'Complete'" style="color: green;">
                        Hoàn thành
                    </label>
                    <label v-if="item.status == 'Proccessing'" style="color: red;">
                        Đã yêu cầu hủy
                    </label>
                    <label v-if="item.status == 'Refunded'" style="color: red;">
                        Đã trả cọc 
                    </label>
                    <label v-if="item.status == 'Canceled'" style="color: red;">
                        Đã hủy chuyến 
                    </label>
                </div>
            </div>
            <hr />
            <div class="giua">
                <div class="trai" style="display:flex;">
                    <img :src="'http://localhost:5027/' + item.post.image"
                        style="height:150px;box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);border-radius:5px;" />
                    <span style="padding:10px 20px;font-size:23px;font-weight:bold;">{{ item.post.name }}</span>
                </div>
                <div class="phai">
                    <p>Số tiền gốc : {{ formatPrice(item.post.pricePerHour) }}/giờ</p>
                    <p>Số tiền gốc : {{ formatPrice(item.post.pricePerDay) }}/ngày</p>
                    <p>Thành tiền : {{ item.total }}</p>
                </div>
            </div>
            <hr />
            <div class="duoi">
                <div class="trai">
                    <a v-if="item.status == 'Accept Booking'" @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });"
                        class="btn nutshort" style="margin-right: 15px;">Thanh toán</a>


                    <a @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });"
                    class="btn nutshort" >Xem chi tiết</a>

                    <a v-if="item.recieveOn <= Date.now && item.status == 'Waiting'" style="margin-left: 20px;" @click="TraCoc(item.id)"
                    class="btn1 nutshort">Yêu cầu trả cọc </a>


                    <!-- <a v-if="item.recieveOn <= Date.now" asp-action="Delete" hidden asp-route-id="@item.Id" style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Yêu cầu trả cọc</a>
                               
                                <a v-else asp-action="Delete" asp-route-id="@item.Id" style="padding:13px 20px;background-color:cadetblue;border-radius:5px;">Yêu cầu trả cọc</a> -->

                </div>
                <div class="phai">
                    <div>
                        Đã đặt cọc : {{ item.prePayment }}
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>

<script>
import BookingVM from '../../../Model/BookingVM';
import BookingService from '../../../Service/api/BookingService';

export default {
    data() {
        return {
            booking: [new BookingVM()]
        }
    },
    methods: {
        async TraCoc(id){
            const response = await BookingService.TraCoc(id);
            console.log("Response của trả cọc: ",response);
            if(response.status){
                this.GetAllBooking();
            }
        },
        async GetAllBooking() {
            const response = await BookingService.GetAllBooking();
            console.log("Booking:", response);
            this.booking = response.data;
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
    },

    async created() {
        await this.GetAllBooking();
    },

}
</script>

<style>
.ulview {
    display: flex;
    gap: 15px;
    margin-top: 7px !important;
    margin-bottom: 20px !important;
}
.ulview li {
    list-style-type: none;
    padding: 4px 8px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
    border-radius: 5px;
}
.tren,
.giua,
.duoi {
    display: flex;
    justify-content: space-between;
    align-items: center;

}
.trai {
    display: flex;
}
</style>

<!-- 
@model IEnumerable<DoAnCNTT.Models.Booking>
    @inject SignInManager<ApplicationUser> SignInManager
    @inject UserManager<ApplicationUser> UserManager
    @{
        ViewData["Title"] = "Lịch sử đặt xe";
        Layout = "~/Views/Shared/_Layout.cshtml";
    
    }
    
    
    @* 
    <table class="table">
        <thead>
            <tr>
                <th>
                    Bài đăng
                </th>
                <th>
                    Ngày nhận
                </th>
                <th>
                    Ngày trả
                </th>
                <th>
                    Tổng tiền
                </th>
                <th>
                    Thành tiền
                </th>
                <th>
                    Đặt cọc
                </th>
                <th>
                    Trạng thái
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
    @foreach (var item in Model) {
            <tr>
                <td>
                    <a asp-action="Details" asp-route-id="@item.PostId">@item.Post.Name</a>
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.RecieveOn)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.ReturnOn)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Total)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.FinalValue)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.PrePayment)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Status)
                </td>
                <td>
                    <a asp-action="Details" asp-route-id="@item.Id">Chi tiết đặt cọc</a> |
                    <a asp-action="Delete" asp-route-id="@item.Id">Yêu cầu trả cọc</a>
                </td>
            </tr>
    }
        </tbody>
    </table>
     *@
    
    
        @* .duoi {
            display: flex;
            justify-content: flex-end;
        }  *@ -->