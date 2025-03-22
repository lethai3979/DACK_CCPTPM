<template>
    <div v-if="bookingPending.length > 0" class="containerPost1" style="margin-left: -30px;">
        <div v-for="item in bookingPending" :key="item.id" class="box postcar">
            <div class="pta" style="width: 280px;height:180px; overflow: hidden;">
                <img :src="'http://localhost:5027/' + item.post.image" :alt="item.id"
                    style="object-fit: cover; width: 100%; height: 100%;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);" />
            </div>
            <router-link style="font-weight: bold;font-size: 20px;margin-left: 10px;"
                :to="{ name: 'user-booking-detail', params: { id: item.id } }">
                Lái xe: {{ item.post.name }}
            </router-link>
            <div class="pte1">
                <div class="giagiam">Bắt đầu {{ formatDatetime(item.recieveOn) }}</div>
                <div class="giagiam">Kết thúc {{ formatDatetime(item.returnOn) }}</div>
                <hr>
                <div class="giagiam" style="display: flex;justify-content: space-between; padding: 0px 20px">
                    <button class="nutnhan" @click="this.$router.push({ name: 'user-booking-detail', params: { id: item.id } });">
                        Xem chi tiết
                    </button>

                </div>
            </div>
        </div>
    </div>

</template>

<script>
import BookingService from '../../../../Service/api/BookingService';
import DetailBooking from './DetailBooking.vue';
export default {
    components: {
        DetailBooking
    },
    data() {
        return {
            bookingPending: [],
            check: true,
            id: 0
        }
    },
    methods: {
        Close() {
            this.check = true;
        },
        async CheckBooking() {
            this.checkBooking = !this.checkBooking;
            try {
                const response = await BookingService.GetAllPenDing();
                this.bookingPending = response.data;
                console.log(response.data);
            }
            catch (error) {
                console.log("Lỗi lấy dữ liệu: ", error);
            }
            console.log("hehe", this.checkBooking);
        },
        formatDatetime(datetimeStr) {
            const date = new Date(datetimeStr);
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0
            const year = date.getFullYear();
            return `${hours}:${minutes} ${day}-${month}-${year}`;
        },
        async ConfirmBooking(bien, id) {
            const response = await BookingService.ConfirmBooking(id, bien);
            console.log(response);
            if (response.success) {
                this.$emit('Close');
                console.log('hehe');
            }
        },
    },
    created() {
        this.CheckBooking();
    },


}
</script>

<style>
.nutnhan{
    padding: 4px 8px;border: 0.5px solid #333;border-radius: 5px;
}
.pte1 {
    padding-top: 5px;
    /* display: flex; */
    color: #434343;
    font-size: .9rem;
    font-weight: 500;
    line-height: 22px;
    margin-left: 15px;
}
</style>