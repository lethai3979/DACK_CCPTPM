<template>

    <div class="containerFa">
        <div class="child">
            <h1>Danh sách yêu thích của bạn</h1>
        </div>
        <div v-if="posts.length == 0" style="font-size: 25px;opacity: 0.8;margin-top: 20px;margin-inline: 5%;">Danh sách trống </div>
        <div class="containerPost1" v-else>
            <router-link :to="{ name: 'user-post-detail', params: { id: item.id } }" class="box postcar"
                v-for="item in posts" :key="item.id">
                <!-- {{ item.post }} -->
                <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                    <img :src="'http://localhost:5027/' + item.post.image" :alt="item.image"
                        style="object-fit: cover; width: 100%; height: 100%;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);" />
                </div>
                <div class="ptb">
                    <div v-if="item.post.gear" class="ptb1 apt" style="padding:2px 4px;">Số tự động</div>
                    <div v-else class="ptb1 apt" style="padding:2px 4px;background-color:darkkhaki;">Số sàn
                    </div>
                </div>

                <div class="ptc">
                    <a style="cursor: pointer;" asp-area="Customer" asp-controller="Posts" asp-action="Details"
                        asp-route-id="@item.Id">{{ item.post.name }}</a>
                </div>
                <div class="ptd">
                    <img src="/src/assets/logoWeb/road-map-line.svg" class="icon_map" alt="">
                    <span class="text_map">{{ item.post.rentLocation }}</span>
                </div>
                <hr>
                <div class="pte">
                    <div class="pte_left">
                        <!-- @* Chưa xong *@ -->
                        <div class="dgsao">
                            <img src="/src/assets/logoWeb/star-s-fill.svg" class="icon_sao_danhgia" alt="">
                            <span class="text_saodanhgia">{{ formatDecimal(item.post.avgRating, 1) }}</span>
                        </div>

                        <div v-if="item.rideNumber > 0" class="sochuyen">
                            <img src="/src/assets/logoWeb/luggage-cart-line.svg" class="icon_map" alt="">
                            <div class="text_sochuyen">{{ item.post.rideNumber }} chuyến</div>
                        </div>

                        <div v-else class="sochuyen">
                            <div class="text_sochuyen">Chưa có chuyến</div>
                        </div>


                    </div>
                    <div class="pte_right">
                        <div class="giagiam">{{ formatPrice(item.post.pricePerHour) }}/giờ</div>
                        <div class="giagiam">{{ formatPrice(item.post.pricePerDay) }}/ngày</div>
                    </div>
                </div>
            </router-link>

        </div>


    </div>

</template>
<script>
import PostVM from '../../../Model/PostVM';
import FavoriteService from '../../../Service/api/FavoriteService';
export default {
    data() {
        return {
            posts: [new PostVM()],
        }
    },
    methods: {
        async getAllPostFavorite() {
            const response = await FavoriteService.getAllFavorite();
            console.log(response);
            this.posts = response.data;
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
    async created() {
        await this.getAllPostFavorite();
    },

}
</script>

<style>
</style>