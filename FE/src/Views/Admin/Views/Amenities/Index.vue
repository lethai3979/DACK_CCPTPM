<template>

    <h1>Danh sách tiện nghi</h1>

    <p>
        <a href="/admin/amenity/create">Thêm mới tiện nghi</a>
    </p>
    <table class="table">
        <thead>
            <tr>
                <th>
                    Tên
                </th>
                <th>
                    Icon
                </th>
                <th>
                    Mã người tạo
                </th>
                <th>
                    Ngày tạo
                </th>
                <th>
                    Mã người chỉnh sửa
                </th>
                <th>
                    Ngày chỉnh sửa
                </th>
                <th>
                    Ẩn
                </th>
                <th>Chức năng</th>
            </tr>
        </thead>

        <tbody>
            <tr v-for="item in amenities" :key="item.id">
                <td>
                    {{ item.name }}
                </td>
                <td>
                    <img :src="'http://localhost:5027/'+item.iconImage" alt="hinhanh.jpg" width="200px" />
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
                        <!-- <router-link :to="'/admin/amenity/edit/' + item.id">Chỉnh
                            sửa</router-link> -->
                        <router-link :to="{ name: 'admin-amenity-edit', params: { id: item.id } }">
                            <button>
                                Chỉnh sửa {{ item.id }}
                            </button>
                        </router-link>
                    </li>
                    <li v-if="item.id">
                        <router-link :to="{ name: 'admin-amenity-detail', params: { id: item.id } }">
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
        const amenities = inject('amenities', ref([]));
        // console.log("Amenities của IndexVue: ",amenities.value);
        return{amenities}
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
<script>
    document.onready
    $(document).ready(function () {
        $("#search-input").keyup(function () {
            var query = $(this).val();
            if (query !== '') {
                $.ajax({
                    url: "/Amenities/SearchSuggestions",
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

</script> -->
