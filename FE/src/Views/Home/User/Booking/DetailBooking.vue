<template>
  <button @click="Close()">X</button>
  <label for="" v-if="booking.id != null">{{ booking.post.name }}</label>
  <label for="" v-if="booking.id != null">{{ booking.recieveOn }}</label>
  <label for="" v-if="booking.id != null">{{ booking.returnOn }}</label>
  <label for="" v-if="booking.id != null">{{ booking.userId }}</label>

  <label for="">Xác nhận cho thuê</label>
  <button @click="ConfirmBooking(true)">Có</button>
  <button @click="ConfirmBooking(false)">Không</button>
</template>

<script>
import BookingVM from '../../../../Model/BookingVM';
import BookingService from '../../../../Service/api/BookingService';
export default {
  data() {
    return {
      PendingDetail: [],
      booking: new BookingVM(),
    }
  },
  methods: {
    Close() {
      this.$emit('Close');
    },
    async ConfirmBooking(bien) {
      const response = await BookingService.ConfirmBooking(this.id, bien);
      console.log(response);
      if (response.success) {
        this.$emit('Close');
        console.log('hehe');
      }
    },
    async CheckBooking() {
      this.checkBooking = !this.checkBooking;
      try {
        const response = await BookingService.GetAllPenDing();
        this.PendingDetail = response.data;
        this.PendingDetail.forEach(p => {
          if (p.id = this.id) {
            this.booking = p;
            console.log(p);
            return;
          }
        }

        )
        console.log(response.data);
      }
      catch (error) {
        console.log("Lỗi lấy dữ liệu: ", error);
      }
      console.log("hehe", this.checkBooking);
    },
  },
  async created() {
    await this.CheckBooking();
    console.log(this.id);
  },
  props: {
    id: {
      type: Number,
      default: 0,
    },
  },

}
</script>

<style></style>