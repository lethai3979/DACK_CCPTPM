<template>
  
<h1>Danh sách loại xe</h1>

<table class="table">
    <thead>
        <tr>
            <th>
                Tên
            </th>
            <th>
                Mã người tạo
            </th>
            <th>
                Ngày tạo
            </th>
            <th>
                Mã người cập nhật
            </th>
            <th>
                Ngày cập nhật
            </th>
            <th>
                Ẩn
            </th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <tr v-for="item in carTypes" :key="item.id">
            <td>
                {{ item.name }}
            </td>
            <td>
                {{ item.createdById }}
            </td>
            <td>
                {{ item.createdOn }}
            </td>
            <td>
                {{ item.modifiedById }}
            </td>
            <td>
                {{ item.modifiedOn }}
            </td>
            <td>
                {{ item.isDeleted }}
            </td>
            <td>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'admin-cartype-edit', params: { id: item.id } }">
                            <button>
                                Chỉnh sửa {{ item.id }}
                            </button>
                        </router-link>
                    </li>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'admin-cartype-detail', params: { id: item.id } }">
                            <button>
                                Chi tiết {{ item.id }}
                            </button>
                        </router-link>
                    </li>
                </td>
        </tr>

    </tbody>
</table>
</template>

<script>
import axios from 'axios';
import { inject,ref } from 'vue';
export default {
    setup(){
        const carTypes = inject('carTypes', ref([]));
        return{carTypes}
    },
    data() {
        return {
            
        }
    },

}
</script>

<style>

</style>

<!-- 
<form asp-controller="Cartypes" class="FormSearch dropdown" asp-action="Index" method="get" style="">
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
                    url: "/Cartypes/SearchSuggestions",
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


 -->
