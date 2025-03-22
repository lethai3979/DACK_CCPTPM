<template>
    
    <div class="bigContai content-item user-profile" style="position: relative;">
        <span class="icon-closeForm">
                <a class="custom-link" onclick="javascript:history.go(-1);">
                    <i class="ri-close-line"></i>
                </a>
            </span>
        <h3 style="margin-inline: 37%;">Thông tin tài khoản</h3>
        <div class="content">
                <div class="avatar-box"><!-- :src="'https://localhost:7265/'+ user.image" -->
                    <div class="avatar1 avatar--xl has-edit" style=" height: 100%;align-items: center;align-content: center;"><img style="width: 200px; height: 200px;object-fit: cover;" loading="lazy" :src="'http://localhost:5027/'+ user.image" :alt="user.name">
                    </div>
                    
                </div> 
                <div class="info-user">
                    <div class="info-box" style="margin-top: 2%;margin-bottom: 0 !important">
                        <div v-if="user.roles && !user.roles.includes('Driver') && user.driver" class="info-box__item">
                            <p>Tên tài xế </p>
                            <p v-if="user.name == null" class="main">----/----/--------</p>
                            <p v-else class="main">{{ user.name }}</p>
                        </div>
                        <div v-else class="info-box__item">
                            <p>Tên khách hàng </p>
                            <p v-if="user.name == null" class="main">----/----/--------</p>
                            <p v-else class="main">{{ user.name }}</p>
                        </div>
                        <div class="info-box__item">
                            <p>Ngày sinh </p>
                            <p v-if="user.birthday == null" class="main">----/----/--------</p>
                            <p v-else class="main">{{ user.birthday }}</p>
                        </div>
                        <div class="info-box__item">
                            <p>Số điện thoại</p>
                            <p v-if="user.phoneNumber !== null" class="main">{{ user.phoneNumber }}</p>
                            <p v-else class="main">Chưa cập nhật</p>

                        </div>
                        <div v-if="user.roles && !user.roles.includes('Driver') && user.driver" class="info-box__item">
                            <p>Số sao</p>
                            <!-- <p v-if="user.name == null" class="main">----/----/--------</p> -->
                            <p class="main">{{ user.driver.ratingPoint }}</p>
                        </div>
                        <div v-if="user.roles && !user.roles.includes('Driver') && user.driver" class="info-box__item">
                            <p>Số tiền mỗi giờ </p>
                            <p class="main">70k/giờ</p>
                            <!-- <p class="main">{{ user.driver.pricePerHour }}</p> -->
                        </div>
                        <!-- <div class="info-box__item">
                            <p>Giấy phép lái xe</p>
                            <p v-if="user.license !== null" class="main">Đã cập nhật</p>
                            <p v-else class="main">Chưa cập nhật</p>
                        </div>
                        <div class="info-box__item">
                            <p>Căn cước công dân</p>
                            <p v-if="user.cic !== null" class="main">Đã cập nhật</p>
                            <p v-else class="main">Chưa cập nhật</p>
                        </div> -->
                    </div>
                    <div class="info-desc" hidden>
                        <div class="info-desc__item">
                            <div class="title-item">Số điện thoại

                            </div>
                            <div v-if="user.phoneNumber !== null" class="name"> {{ user.phoneNumber }}
                            </div>
                            <div class="name"> Chưa cập nhật
                            </div>
                        </div>
                    </div>
                </div>
                <div class="info-note"></div>
            </div>
        <hr style="width:100%;" />
        <div style="margin-left:10px;">
            <h3 style="margin-left:10px;">Các bài viết khác</h3>
            <div class="py-2">
                <div class="" >
                    <div class="row gx-4 gx-lg-5 row-cols-2 row-cols-md-3 row-cols-xl-4 justify_content-center">
                        <div class="containerPost1" style="background-color: none;" v-if="posts.length > 0">
                            <router-link :to="{ name: 'user-post-detail', params: { id: item.id } }" class="box postcar" style="margin-inline: 20px;"
                                v-for="item in posts" :key="item.id">
                                <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                                    <img :src="'http://localhost:5027/' + item.image" :alt="item.image"
                                        style="object-fit: cover; width: 100%; height: 100%;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);" />
                                </div>
                                <div class="ptb">
                                    <div v-if="item.gear" class="ptb1 apt" style="padding:2px 4px;">Số tự động</div>
                                    <div v-else class="ptb1 apt" style="padding:2px 4px;background-color:darkkhaki;">Số
                                        sàn
                                    </div>
                                </div>

                                <div class="ptc">
                                    <a style="cursor: pointer;" asp-area="Customer" asp-controller="Posts"
                                        asp-action="Details" asp-route-id="@item.Id">{{ item.name }}</a>
                                </div>
                                <div class="ptd">
                                    <img src="/src/assets/logoWeb/road-map-line.svg" class="icon_map" alt="">
                                    <span class="text_map">{{ item.rentLocation }}</span>
                                </div>
                                <hr>
                                <div class="pte">
                                    <div class="pte_left">
                                        <!-- @* Chưa xong *@ -->
                                        <div class="dgsao">
                                            <img src="/src/assets/logoWeb/star-s-fill.svg" class="icon_sao_danhgia"
                                                alt="">
                                            <span class="text_saodanhgia">{{ formatDecimal(item.avgRating, 1) }}</span>
                                        </div>

                                        <div v-if="item.rideNumber > 0" class="sochuyen">
                                            <img src="/src/assets/logoWeb/luggage-cart-line.svg" class="icon_map"
                                                alt="">
                                            <div class="text_sochuyen">{{ item.rideNumber }} chuyến</div>
                                        </div>

                                        <div v-else class="sochuyen">
                                            <div class="text_sochuyen">Chưa có chuyến</div>
                                        </div>


                                    </div>
                                    <div class="pte_right">
                                        <div class="giagiam">{{ formatPrice(item.pricePerHour) }}/giờ</div>
                                        <div class="giagiam">{{ formatPrice(item.pricePerDay) }}/ngày</div>
                                    </div>
                                </div>
                            </router-link>

                        </div>
                        <span v-else>
                            Chưa có bài viết
                        </span>

                    </div>
                </div>
            </div>
            <div>
                <a v-if="user.roles == 'Admin'" class="btn1 nutshort">Khóa tài khoản</a>
            </div>
        </div>
    </div>

</template>

<script>
import PostVM from '../../Model/PostVM';
import UserVM from '../../Model/UserVM';
import AuthenticationService from '../../Service/api/AuthenticationService';
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
            const userId = this.$route.params.userId;
            const response = await AuthenticationService.FindUser(userId);
            this.user = response.data;
            console.log("User Find: ", this.user);
            const responsePost = await PostService.getAllPostByFindUserId(userId);
            this.posts = responsePost.data;
            console.log(responsePost)
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
        formatDecimal(number, decimalPlaces) {
            const factor = Math.pow(10, decimalPlaces);
            return Math.ceil(number * factor) / factor;
        },
    },
    created() {
        this.getUser();
    },

}
</script>

<style>

.error {
    color: red;
}

h5.check {
    font-weight: 100 !important;
    color: #333333cc;
}

hr.check {
    background-color: var(--bg-green2);
    padding: 2px;
}

.title-edit {
    margin-top: -15px;
    display: flex;
    gap: 20px;
}

.content-item {
    padding: 24px;
}


.user-profile .content {
    display: flex;
    grid-gap: 36px;
    gap: 36px;
}

.user-profile .content .avatar-box {
    display: flex;
    flex-direction: column;
    align-items: center;
    grid-gap: 16px;
    gap: 16px;
    width: 30%;
}

.user-profile .content .avatar-box h6 {
    text-align: center;
}

.user-profile {
    display: flex;
    flex-direction: column;
    grid-gap: 24px;
    gap: 24px;
}

.content-item {
    background: #fff;
    border-radius: 12px;
    padding: 24px 32px;
}

.user-profile .title {
    display: flex;
    align-items: center;
    justify-content: space-between;
    grid-gap: 8px;
    gap: 8px;
}

h5 {
    font-size: 1.5rem;
    font-weight: 700;
}

.user-profile .title .total-trips {
    display: flex;
    align-items: baseline;
    padding: 8px 16px;
    grid-gap: 4px;
    gap: 4px;
    border-radius: 8px;
    border: 1px solid #e0e0e0;
}


.user-profile .content .info-user .info-box {
    display: flex;
    flex-direction: column;
    grid-gap: 16px;
    gap: 16px;
    padding: 16px 24px;
    border-radius: 8px;
    background: #f6f6f6;
}

.user-profile .content .info-user .info-box__item {
    display: flex;
    align-items: baseline;
    justify-content: space-between;
}

.user-profile .content .info-user .info-desc {
    display: flex;
    flex-direction: column;
    grid-gap: 16px;
    gap: 16px;
}

.user-profile .content .info-user .info-desc__item {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    grid-gap: 4px;
    gap: 4px;
}

.user-profile .content .info-user {
    display: flex;
    flex-direction: column;
    width: 70%;
    grid-gap: 24px;
    gap: 24px;
}

.avatar1 img {
    width: 150px;
    height: 150px;
    border: 1px solid;
    object-fit: cover;
    border-radius: 100%;
}

.trip-class {
    display: flex;
    gap: 20px;
}
.bigContai {
    border-radius: 5px;
    width: 85%;
    background-color: white;
    margin: auto;
    padding: 20px 30px;
}
</style>

<!-- 
@model ﻿DoAnCNTT.Models.ApplicationUser
@using Microsoft.AspNetCore.Identity
@inject SignInManager<ApplicationUser> SignInManager
@inject UserManager<ApplicationUser> UserManager

@{
    ViewData["Title"] = "Thông tin khách hàng";
    Layout = "/Views/Shared/_Layout.cshtml";
}
@{
    var listpost = ViewBag.listpost as List<DoAnCNTT.Models.Post>;

    var userBooking = ViewBag.userBooking as List<DoAnCNTT.Models.Booking>;
}



<div class="bigContai">
    <div style="display:flex;justify-content:space-between;background-color:antiquewhite;padding:15px 20px;">
        <div style="display:flex;">
            <a href="javascript:history.go(-1);" style="margin-right:0px;">
                <img style="width:30px;margin-top:2px;" src="@Url.Content("~/images/logo/muiten.png")" alt="" /><a>
            <h3>
                 @Model.Name
            </h3>
        </div>
        </div>
    <div style="display:flex;">
       @*  justify-content:space-between; *@
        <div>
            <img style="border-radius:50%;width:220px;height:220px;" src="@Url.Content(Model.Image)" alt="User Image" />
        </div>
        <div style="margin-left:20px;">
            <h2>Tên của khách hàng : @Model.Name</h2>
            @if(ViewBag.thue == true)
            {
                <h4>Thông tin liên hệ </h4>    
                <p>Số điện thoại: @Model.PhoneNumber</p>
            }
            @if (User.IsInRole("Admin"))
            {
                <a class="nav-link den" asp-area="Admin" asp-controller="Customers" asp-action="GetBookingHistory" asp-route-userId="@Model.Id">Lịch sử đặt xe</a>
                <a class="nav-link den" asp-area="Admin" asp-controller="Customers" asp-action="GetPaymentHistory" asp-route-userId="@Model.Id">Lịch sử giao dịch</a>
            }
            else
            {
                <a hidden href="#"></a>
                <a hidden href="#"></a>
            }
        </div>
    </div>
    <hr style="width:100%;"/>
    <div style="margin-left:10px;" >
        <h3 style="margin-left:10px;">Các bài viết khác</h3>
        <section class="py-2">
            <div class="container ">
                <div class="row gx-4 gx-lg-5 row-cols-2 row-cols-md-3 row-cols-xl-4 justify_content-center">

                    @if (listpost != null && listpost.Count > 0)
                    {
                        @foreach (var item in listpost)
                        {

                            <div class="box" style="padding:3px;margin-inline:8.7px;background-color:white;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); margin-bottom:15px;">
                        <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                            <img src="@item.Image" alt="" style="object-fit: cover; width: 100%; height: 100%;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);" />
                        </div>
                        <div class="ptb">
                            @if (item.Gear)
                            {
                                <div class="ptb1 apt" style="padding:2px 4px;">Số tự động</div>
                            }
                            else
                            {
                                <div class="ptb1 apt" style="padding:2px 4px;">Số sàn</div>
                            }

                        </div>

                        <div class="ptc"><a asp-area="Customer" asp-controller="Posts" asp-action="Details" asp-route-id="@item.Id">@Html.DisplayFor(modelItem => item.Name)</a></div>
                        <div class="ptd">
                            <img src="@Url.Content("~/images/logo/road-map-line.svg")" class="icon_map" alt="">
                            <span class="text_map">@Html.DisplayFor(modelItem => item.RentLocation)</span>
                        </div>
                        <hr>
                        <div class="pte">
                            <div class="pte_left">
                                @*  Chưa xong *@
                                <div class="dgsao">
                                    <img src="@Url.Content("~/images/logo/star-s-fill.svg")" class="icon_sao_danhgia" alt="">
                                    <span class="text_saodanhgia">@item.AvgRating</span>
                                </div>
                                @if (item.RideNumber > 0)
                                {
                                    <div class="sochuyen">
                                        <img src="@Url.Content("~/images/logo/luggage-cart-line.svg")" class="icon_map" alt="">
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
                    }
                    else
                    {
                        <span>
                            Chưa có bài viết
                        </span>
                    }
                </div>
            </div>
    </div>
    <div>
        <a class="nav-link den" asp-area="Admin" asp-controller="Customers" asp-action="LockAccount" asp-route-userId="@Model.Id">Khóa tài khoản</a>
    </div>
    </section>
</div>


<script>
    
        function formatPrice(price) {
    if (price >= 1000) {
        return (price / 1000) + 'k';
    }
    return price;
}

$(document).ready(function () {
    $('.giagiam').each(function () {
        var price = parseFloat($(this).text().replace(/[^0-9.-]+/g, ""));
        $(this).text(formatPrice(price) + '/giờ');
    });
});
    
</script>


<style>
    .bigContai {
        border-radius:5px;
        width: 85%;
        background-color: white;
        margin: auto;
        @* padding: 25px; *@
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
            background-color: aquamarine;
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

</style> -->