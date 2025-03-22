<template>
    <div>
        <h1>Details</h1>

        <div>
            <h4>Booking</h4>
            <hr />
            <dl class="row" v-if="booking.id != null">
                <dt class="col-sm-2">
                    Đặt cọc
                </dt>
                <dd class="col-sm-10">
                    {{ booking.prePayment }}
                    <!-- @Html.DisplayFor(model => model.PrePayment) -->
                </dd>
                <dt class="col-sm-2">
                    Tổng tiền
                    <!-- @Html.DisplayNameFor(model => model.Total) -->
                </dt>
                <dd class="col-sm-10">
                    {{ booking.total }}
                    <!-- @Html.DisplayFor(model => model.Total) -->
                </dd>
                <dt class="col-sm-2">
                    Thành tiền
                    <!-- @Html.DisplayNameFor(model => model.FinalValue) -->
                </dt>
                <dd class="col-sm-10">
                    {{ booking.finalValue }}
                    <!-- @Html.DisplayFor(model => model.FinalValue) -->
                </dd>
                <dt class="col-sm-2">
                    Ngày nhận
                    <!-- @Html.DisplayNameFor(model => model.RecieveOn) -->
                </dt>
                <dd class="col-sm-10">
                    {{ booking.recieveOn }}
                    <!-- @Html.DisplayFor(model => model.RecieveOn) -->
                </dd>
                <dt class="col-sm-2">
                    Ngày trả
                    <!-- @Html.DisplayNameFor(model => model.ReturnOn) -->
                </dt>
                <dd class="col-sm-10">
                    {{ booking.returnOn }}
                    <!-- @Html.DisplayFor(model => model.ReturnOn) -->
                </dd>
                <dt class="col-sm-2">
                    Thanh toán
                    <!-- @Html.DisplayNameFor(model => model.IsPay) -->
                </dt>
                <dd class="col-sm-10">
                    <p v-if="booking.isPay">Đã thanh toán</p>
                    <p v-else>Chưa thanh toán</p>
                </dd>
                <dt class="col-sm-2">
                    Trạng thái
                    <!-- @Html.DisplayNameFor(model => model.Status) -->
                </dt>
                <dd class="col-sm-10">
                    {{ booking.status }}
                    <!-- @Html.DisplayFor(model => model.Status) -->
                </dd>

            </dl>
        </div>
        <div>
            <a class="btn nutshort" @click="SubmitRequest(booking.id, true)">Trả cọc {{ booking.id }}</a> |
            <a class="btn1 nutshort" @click="SubmitRequest(booking.id, false)">Không trả cọc</a> |
            <a @click="$router.go(-1)">Back to List</a>
        </div>

    </div>
</template>


<script>
import BookingVM from '../../../../Model/BookingVM';
import BookingRequestService from '../../../../Service/api/BookingRequestService';

export default {
    data() {
        return {
            BookingRequest: [],
            booking: new BookingVM(),
            
        }
    },
    methods: {
        async getAllRequest() {
            const id = this.$route.params.id;
            console.log(id);
            const response = await BookingRequestService.getAllCancel();
            console.log("Response details:", response);
            if (response.success) {
                this.BookingRequest = response.data;
                this.BookingRequest.forEach(p => {
                    if (id == p.id) {
                        this.booking = p;
                        console.log(this.booking);
                    }
                })
            }
        },
        async SubmitRequest(bookingid, bien) {
            console.log(bookingid, bien);
            const response = await BookingRequestService.ConfirmBookingRquest(bookingid, bien);
            console.log("Response submit request: ", response);
            if (response.status == 200) {
                console.log("Thanh cong");
                this.$router.go(-1);
            }
        }
    },
    async created() {
        await this.getAllRequest();
    },


}
</script>

<style></style>
