<template>
    <h1>Danh sách khuyến mãi</h1>

    <table class="table">
        <thead>
            <tr>
                <th>
                    Nội dung
                </th>
                <th>
                    Giá trị khuyến mãi
                </th>
                <th>
                    Ngày kết thúc khuyến mãi
                </th>
                <th>
                    Ngày tạo
                </th>
                <th>
                    Ngày cập nhật
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody v-if="promotions != null">
            <tr v-for="item in promotions" :key="item">
                <td>
                    <!-- <a asp-action="Details" asp-route-id="@item.Id">@Html.DisplayFor(modelItem => item.Content)</a> -->
                    {{ item.content }}
                </td>
                <td>
                    {{ item.discountValue }}
                   
                </td>
                <td>
                    {{ item.expiredDate }}
                   
                </td>
                <td>
                    {{ item.createdOn }}
                   
                </td>
                <td>
                    {{ item.modifiedOn }}
                   
                </td>
                <td>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'admin-promotion-edit', params: { id: item.id } }">
                            <button>
                                Chỉnh sửa {{ item.id }}
                            </button>
                        </router-link>
                    </li>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'admin-promotion-detail', params: { id: item.id } }">
                            <button>
                                Chi tiết {{ item.id }}
                            </button>
                        </router-link>
                    </li>
                </td>
            </tr>
            
        </tbody>
        <tbody v-else>
            <tr>Danh sách khuyến mãi trống</tr>
        </tbody>
    </table>

</template>

<script>

import PromotionService from '../../../../Service/api/PromotionService';
import PromotionVM from '../../../../Model/PromotionVM';
import axios from 'axios';
export default {
    setup() {
    },
    data() {
        return {
            promotions: null,
        }
    },
    methods: {
        async getAll() {
            const token = sessionStorage.getItem("authToken");
            try{
                const response = await axios.get(
                `http://localhost:5027/api/AdminPromotion/GetAllAdminPromotion`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
            this.promotions = response.data.data;
            }catch(error){
                console.log(error);
            }
        

        }
    },
    created() {
        this.getAll();

    }
}
</script>

<style>
td .a {
    padding: 10px 15px;
    border-radius: 5px;
    background-color: olivedrab;
}
 .table {
    width: 100%;
    border-collapse: collapse;
    margin-block: 15px;
}

    .table td {
        margin-left:15px;
        border: 1px solid #ddd;
        padding: 13px;
        text-align: left;
        border: 1px solid #ccc;
    }

    .table th {
        text-align:center;
        background-color: #ccffcc;
    }

    .table tr:nth-child(1) td {
        font-weight: bold;
        padding: 10px;
    }
tbody tr {
    border: 1px solid #ccc; 
}

    tbody tr td {
        padding: 10px; 
    }
a {
    text-decoration: none;
    color: black;
    margin-inline:15px;
} 
</style>
<!-- 
<form asp-controller="Promotions" class="FormSearch dropdown" asp-action="Index" method="get" style="">
    <input id="search-input" style="border:none;" class="InfoSearch" type="text" name="query" placeholder="Tìm Sản phẩm" />
    <div id="search-results" class="dropdown-content">
    </div>
    <input class="ButtonSearch" type="submit" value="Search" />
</form>



<script>
    document.onready
    $(document).ready(function () {
        $("#search-input").keyup(function () {
            var query = $(this).val();
            if (query !== '') {
                $.ajax({
                    url: "/Promotions/SearchSuggestions",
                    type: "GET",
                    data: { query: query },
                    dataType: "json",
                    success: function (data) {
                        $("#search-results").empty();
                        $.each(data, function (index, suggestion) {
                            $("#search-results").append("<div class='suggestion'>" + suggestion + "</div>");
                        });
                        debugger;
                        // console.log($("#search-results"))
                        if ($("#search-results").children().length == 0) {
                            $(".dropdown-content").css("display", "none");
                        } else {
                            $(".dropdown-content").css("display", "block");
                        }
                    },
                    error: function (xhr, status, error) {
                        console.log("Error:", error);
                    }
                });
            } else {
                $("#search-results").empty();
                $(".dropdown-content").css("display", "none");
            }
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

</script>


                <style>
                    
                </style> -->