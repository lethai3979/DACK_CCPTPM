﻿<template>
    <div v-if="user.roles.includes('Admin')">
        <nav style="border-radius:15px;padding:20px 30px; margin-top:20px;" class="SearchCar">
            <!-- Form tìm kiếm cho khách hàng -->
            <form asp-area="" asp-controller="Home" asp-action="ViewSearch" method="get" style="">
                <label class="la">Tìm xe</label>
                <div style="display:flex;justify-content:space-between;">
                    <div style="display:flex;" class="mar">
                        <label class="label1">Hãng xe: </label>
                        <select id="company-input" class="select" name="company"
                            style="width: 100%; border:none;border-radius:5px !important;">
                            <option value="">Chọn xe cần tìm</option>
                            <!-- Các hãng xe sẽ được load từ backend -->
                        </select>
                    </div>
                    <div class="volume-slider mar">
                        <label class="label">Số chổ ngồi: <span id="rangeValue">2</span></label>
                        <input type="range" min="2" max="16" value="0" step="1" class="slider" name="seat"
                            id="volumeRange">
                    </div>
                    <div style="display:flex;" class="mar">
                        <label class="label">Động cơ: </label>
                        <select name="gear" class="form-control">
                            <option value="">Loại động cơ</option>
                            <option value="Số sàn">Số sàn</option>
                            <option value="Số tự động">Số tự động</option>
                        </select>
                    </div>
                    <div style="display:flex;" class="mar">
                        <label class="label" style="margin-right:10px; width:170px;">Nhiên liệu: </label>
                        <select name="Fuel" class="form-control">
                            <option value="">Loại nhiên liệu</option>
                            <option value="Xăng">Xăng</option>
                            <option value="Dầu">Dầu</option>
                            <option value="Điện">Điện</option>
                        </select>
                    </div>
                    <div class="mar" hidden>
                        <label class="label"> Tài Xế Riêng:</label>
                        <label class="" style="margin-left:-30px;">
                            <input class="checkbox" style="width:30px;height:30px;" type="checkbox" name="HasDriver"
                                value="false" onclick="updateCheckboxValue(this)" />
                        </label>
                    </div>
                </div>
                <div id="search-results" class="dropdown-content"></div>
                <input class="ButtonSearch" type="submit" value="Tìm kiếm" />
            </form>

        </nav>

        <h1>Danh sách bài đăng</h1>

        <table style="min-height:500px;" class="table">
            <thead>
                <tr>
                    <th>Tên</th>
                    <th>Hình ảnh</th>
                    <th>Động cơ</th>
                    <th>Loại nhiên liệu</th>
                    <th>Địa chỉ nhận</th>
                    <th>Có tài xế</th>
                    <th>Giá</th>
                    <th></th>
                </tr>
            </thead>
            <tbody v-if="posts.length > 0">
                @foreach (var item in Model)
                {
                <tr v-for="item in posts" :key="item.id">
                    <td>
                        <a asp-controller="Posts" asp-area="Customer" asp-action="Details"
                            asp-route-id="@item.Id">@Html.DisplayFor(modelItem => item.Name)</a>
                    </td>
                    <td>
                        <img src="@item.Image" alt="hinhanh.jpg" style="width: 200px" />
                    </td>


                    @if (item.Gear)
                    {
                    <td style="padding:2px 4px;">Số tự động</td>
                    }
                    else
                    {
                    <td style="padding:2px 4px;background-color:darkkhaki;">Số sàn</td>
                    }



                    <td>@Html.DisplayFor(modelItem => item.Fuel)</td>

                    <td>@Html.DisplayFor(modelItem => item.RentLocation)</td>
                    <td>@Html.DisplayFor(modelItem => item.HasDriver)</td>
                    <td>@Html.DisplayFor(modelItem => item.Price)</td>
                    <td>
                        <a asp-action="Delete" asp-route-id="@item.Id">Xóa</a>
                    </td>
                </tr>
                }
            </tbody>
            <tbody v-else>Danh sách xe trống</tbody>
        </table>

    </div>
    <div v-if="!user.roles || user.roles.includes('User')">
        <div>
            <div>
                <a @click="this.$router.push({ path: '/Home' })">Quay lại</a>
            </div>
            <nav style="border-radius:15px;padding:20px 30px; margin-top:20px;margin-bottom:20px;" class="SearchCar">
                <!-- Form tìm kiếm cho khách hàng -->
                <form asp-area="" asp-controller="Home" asp-action="ViewSearch" method="get" style="">
                    <label class="la">Tìm xe cho bạn</label>
                    <div style="display:flex;justify-content:space-between;">
                        <div style="display:flex;" class="mar">
                            <label class="label1">Hãng xe: </label>
                            <select id="company-input" class="select" name="company" style="width: 100%; border:none;">
                                <option value="">Chọn xe cần tìm</option>
                                <!-- Các hãng xe sẽ được load từ backend -->
                            </select>
                        </div>
                        <div class="volume-slider mar">
                            <label class="label">Số chổ ngồi: <span id="rangeValue">2</span></label>
                            <input type="range" min="2" max="16" value="0" step="1" class="slider" name="seat"
                                id="volumeRange">
                        </div>
                        <div style="display:flex;" class="mar">
                            <label class="label">Động cơ: </label>
                            <select name="gear" class="form-control">
                                <option value="">Loại động cơ</option>
                                <option value="Số sàn">Số sàn</option>
                                <option value="Số tự động">Số tự động</option>
                            </select>
                        </div>
                        <div style="display:flex;" class="mar">
                            <label class="label" style="margin-right:10px; width:170px;">Nhiên liệu: </label>
                            <select name="Fuel" class="form-control">
                                <option value="">Loại nhiên liệu</option>
                                <option value="Xăng">Xăng</option>
                                <option value="Dầu">Dầu</option>
                                <option value="Điện">Điện</option>
                            </select>
                        </div>
                        <div class="mar">
                            <label class="label"> Tài Xế Riêng:</label>
                            <label class="" style="margin-left:-30px;">
                                <input class="checkbox" style="width:30px;height:30px;" type="checkbox" name="HasDriver"
                                    value="false" onclick="updateCheckboxValue(this)" />
                            </label>
                        </div>
                    </div>
                    <div id="search-results" class="dropdown-content"></div>
                    <input class="ButtonSearch" type="submit" value="Tìm kiếm" />
                </form>
                }
            </nav>
            <!-- Sắp xếp và hiển thị danh sách xe -->
            <section class="py-2" style="min-height:600px;">
                <!-- Code sắp xếp và hiển thị danh sách xe -->
                @{
                //Sắp xếp theo giá
                bool nextSortPriceOrder = ViewBag.SortPriceOrder != null ? !(bool)ViewBag.SortPriceOrder : true;
                string sortPriceText = ViewBag.SortPriceOrder != null && (bool)ViewBag.SortPriceOrder ? "Giá giảm dần" :
                "Giá
                tăng dần";

                //Sắp xếp theo số chuyến
                bool nextSortRideNumberOrder = ViewBag.SortRideNumberOrder != null ? !(bool)ViewBag.SortRideNumberOrder
                : true;
                string sortRideNumberText = ViewBag.SortRideNumberOrder != null && (bool)ViewBag.SortRideNumberOrder ?
                "Số
                chuyến giảm dần" : "Số chuyến tăng dần";

                //Sắp xếp theo đánh giá
                //Sắp xếp theo số chuyến
                bool nextSorRatingOrder = ViewBag.SortRatingOrder != null ? !(bool)ViewBag.SortRatingOrder : true;
                string sortRatingText = ViewBag.SortRatingOrder != null && (bool)ViewBag.SortRatingOrder ? "Đánh giá
                giảm dần" :
                "Đánh giá tăng dần";


                bool newposts = ViewBag.SortByNewPosts != null ? !(bool)ViewBag.SortByNewPosts : true;
                string sortnewpostsText = ViewBag.SortByNewPosts != null && (bool)ViewBag.SortByNewPosts ? "Bài đăng mới
                nhất" :
                "Bài đăng cũ nhất";
                }
                <i style="font-size:20px;font-weight:bold;margin-left:20px;">Lọc :</i>
                <a class="theloc" asp-action="SortByPrice" asp-route-sortPriceOrder="@nextSortPriceOrder">
                    @if (@sortPriceText == "Giá giảm dần")
                    {
                    @sortPriceText

                    <img width="20px" src="/src/assets/logoWeb/tiengiam.png" />
                    }
                    else
                    {
                    @sortPriceText

                    <img width="20px" src="/src/assets/logoWeb/tientang.png" />
                    }

                </a>
                <a class="theloc" asp-action="SortByRideNumber"
                    asp-route-sortRideNumberOrder="@nextSortRideNumberOrder">
                    @if (@sortRideNumberText == "Số chuyến giảm dần")
                    {
                    @sortRideNumberText
                    <img width="20px" src="/src/assets/logoWeb/giamdan.png" />
                    }
                    else
                    {
                    @sortRideNumberText
                    <img width="20px" src="/src/assets/logoWeb/tangdan.png" />
                    }
                </a>
                <a class="theloc" asp-action="SortByRating" asp-route-sortRatingOrder="@nextSorRatingOrder">
                    @if (@sortRatingText == "Đánh giá giảm dần")
                    {
                    @sortRatingText
                    <img width="20px" src="/src/assets/logoWeb/danhgiathap.png" />
                    }
                    else
                    {
                    @sortRatingText
                    <img width="20px" src="/src/assets/logoWeb/danhgiacao.png" />
                    }
                </a>
                <a class="theloc" asp-action="SortByNewPosts" asp-route-newposts="@newposts">
                    @if (@sortnewpostsText == "Bài đăng mới nhất")
                    {
                    @sortnewpostsText
                    <img width="20px" src="/src/assets/logoWeb/time1.png" />
                    }
                    else
                    {
                    @sortnewpostsText
                    <img width="20px" src="/src/assets/logoWeb/time2.png" />
                    }
                </a>
                <div class="container" style="margin-top:20px;">
                    <div class="row gx-4 gx-lg-5 row-cols-2 row-cols-md-3 row-cols-xl-4 justify_content-center">

                        @foreach (var item in Model)
                        {

                        <div class="box"
                            style="padding:3px;margin-left:20px;background-color:white;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);margin-bottom:20px;">
                            <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                                <img src="@item.Image" alt=""
                                    style="object-fit: cover; width: 100%; height: 100%;border:0.5px solid #1ee156;" />
                            </div>
                            <div class="ptb">
                                @if (item.Gear)
                                {
                                <div class="ptb1 apt" style="padding:2px 4px;">Số tự động</div>
                                }
                                else
                                {
                                <div class="ptb1 apt" style="padding:2px 4px;background-color:darkkhaki;">Số sàn</div>
                                }

                            </div>

                            <div class="ptc"><a asp-area="Customer" asp-controller="Posts" asp-action="Details"
                                    asp-route-id="@item.Id">@Html.DisplayFor(modelItem => item.Name)</a></div>
                            <div class="ptd">
                                <img src="/src/assets/logoWeb/road-map-line.svg" class="icon_map" alt="">
                                <span class="text_map">@Html.DisplayFor(modelItem => item.RentLocation)</span>
                            </div>
                            <hr>
                            <div class="pte">
                                <div class="pte_left">

                                    <div class="dgsao">
                                        <img src="/src/assets/logoWeb/star-s-fill.svg" class="icon_sao_danhgia" alt="">
                                        <span class="text_saodanhgia">@item.AvgRating</span>
                                    </div>
                                    @if (item.RideNumber > 0)
                                    {
                                    <div class="sochuyen">
                                        <img src="/src/assets/logoWeb/luggage-cart-line.svg" class="icon_map" alt="">
                                        <div class="text_sochuyen">@item.RideNumber chuyến</div>
                                    </div>
                                    }
                                    else
                                    {
                                    <div class="sochuyen">
                                        Chưa có chuyến
                                    </div>
                                    }

                                </div>
                                <div class="pte_right">
                                    <div class="giagiam">@item.Price/giờ</div>
                                </div>
                            </div>

                        </div>
                        }
                    </div>
                </div>
            </section>

            <!-- Pagination -->
            <div class="nutchuyentrang">
                <a asp-action="ViewSearch" asp-route-company="@ViewBag.Company" asp-route-seat="@ViewBag.Seat"
                    asp-route-gear="@ViewBag.Gear" asp-route-hasDriver="@ViewBag.HasDriver"
                    asp-route-pageNumber="@(Model.PageIndex - 1)" class="btn btn-default @prevDisabled">
                    <img src="/src/assets/logoWeb/back-arrow.png" width="25px" /> Previous
                </a>
                <a asp-action="ViewSearch" asp-route-pageNumber="@(Model.PageIndex + 1)"
                    asp-route-company="@ViewBag.Company" asp-route-seat="@ViewBag.Seat" asp-route-gear="@ViewBag.Gear"
                    asp-route-hasDriver="@ViewBag.HasDriver" class="btn btn-default @nextDisabled">
                    Next <img src="/src/assets/logoWeb/go-arrow.png" width="25px" />
                </a>
            </div>

        </div>

    </div>




</template>

<script>
import UserVM from '../../Model/UserVM';
import PostService from '../../Service/api/PostService';
export default {
    data() {
        return {
            user: new UserVM(),
            posts: [new PostVM()]
        }
    },
    methods: {
        async getUser() {
            const response = await AuthenticationService.getUser();
            this.user = response;
        },
        async getAllPost() {
            const response = await PostService.getAllPost();
            this.posts = response.data;
        }
    },
    created() {
        this.getUser();
    },


}
</script>

<style></style>











@model PaginatedList<DoAnCNTT.Models.Post>
@{
    ViewData["Title"] = "Trang chủ";
    Layout = "~/Views/Shared/_Layout.cshtml";
}
@{
    var prevDisabled = !Model.HasPreviousPage ? "disabled" : "";
    var nextDisabled = !Model.HasNextPage ? "disabled" : "";
}



<script>

    document.getElementById('volumeRange').addEventListener('input', function () {
        document.getElementById('rangeValue').innerText = this.value;
    });
    function updateCheckboxValue(checkbox) {
        if (checkbox.checked) {
            checkbox.value = true;
        }
        else {
            checkbox.value = false;
        }
    }
    $(document).ready(function () {

        $.ajax({
            url: '/Home/GetCarCompanies',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var $select = $('#company-input');
                $select.empty(); // Xóa các option hiện tại
                $select.append('<option value="">Chọn xe cần tìm</option>'); // Thêm option mặc định

                // Duyệt qua danh sách các hãng xe và thêm vào dropdown
                $.each(data, function (index, company) {
                    $select.append('<option value="' + company + '">' + company + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
            }
        });
        $(".prev-button").click(function () {
            var lastSlide = $(".slide").last();
            $(".slider-container").prepend(lastSlide);
        });

        $(".next-button").click(function () {
            var firstSlide = $(".slide").first();
            $(".slider-container").append(firstSlide);
        });
        $(document).on("click", ".suggestion", function () {
            var selectedSuggestion = $(this).text();
            $("#search-input").val(selectedSuggestion);
            $("#search-results").empty();
            $(".dropdown-content").css("display", "none");
        });
        $(document).click(function (event) {
            if (!$(event.target).closest('.dropdown').length) {
                $(".dropdown-content").css("display", "none");
            }
        });
        $(document).ready(function () {
            $(".header").click(function () {
                $(this).siblings(".item").slideToggle();
            });
        });
    });
    function formatPrice(price) {
        if (price >= 1000) {
            return (price / 1000) + 'k';
        }
        return price;
    }

    $(document).ready(function () {
        // Định dạng tất cả các phần tử có class 'giagiam'
        $('.giagiam').each(function () {
            var price = parseFloat($(this).text().replace(/[^0-9.-]+/g, ""));
            $(this).text(formatPrice(price) + '/giờ');
        });
    });


</script>


<style>
    .select{
        border-radius: 5px !important;
    }
    .nutchuyentrang {
        margin-inline: 43%;
        width: 300px;
    }

    .nutchuyentrang1 {
        margin-left: 75%;
        width: 300px;
    }

    .box {
        width: 305px;
        height: 345px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        border-radius: 15px;
    }

    .pta {
        padding: 0px;
        margin: 10px;
    }

        .pta img {
            border-radius: 15px;
            width: 280px;
            height: 180px;
        }

    .ptb {
        padding: 0px;
        display: flex;
        width: 100%;
        margin: 0;
    }

    .ptb1 {
        font-size: 13px;
        border-radius: 50px;
        margin: 0;
    }

        .ptb1.apt {
            background-color: rgb(50, 175, 134);
            color:white;
        }

        .ptb1.bpt {
            background-color: rgb(243, 253, 49);
        }

        .ptb1.cpt {
            background-color: rgb(79, 143, 255);
        }

    .ptb div {
        padding: 0px;
        margin-inline: 10px;
    }

    .ptc {
        padding: 0px;
        margin-block: 7px;
    }

    .ptd {
        padding: 0px;
        margin-inline: 10px;
        display: flex;
    }

        .ptd .icon_map {
            width: 25px;
        }

        .ptd .text_map {
            padding-top: 3px;
        }

    hr {
        width: 275px;
        margin-top: 10px;
        margin-bottom: 5px;
    @* margin:5px; *@
    }

    .pte {
        padding: 0px;
        margin-inline: 10px;
        display: flex;
        justify-content: space-between;
    }

        .pte .pte_left {
            display: flex;
        }

            .pte .pte_left .dgsao {
                display: flex;
                margin-right: 10px;
            }

            .pte .pte_left .sochuyen {
                display: flex;
            }

                .pte .pte_left .sochuyen .icon_map {
                    width: 19px;
                }

            .pte .pte_left .dgsao .icon_sao_danhgia {
                width: 18px;
                /* color: rgb(253, 236, 0); */
            }

        .pte .pte_right {
            display: flex;
        }

            .pte .pte_right div {
                margin-right: 10px;
            }
</style>

