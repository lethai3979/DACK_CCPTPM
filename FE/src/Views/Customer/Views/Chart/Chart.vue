<template>
    <div class="Login">
        <div style="padding: 20px 30px;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);border-radius: 15px;position: relative;">
            <span class="icon-closeForm" style="margin-right: 0px !important;">
                <a class="custom-link" @click="Close()">
                    <i class="ri-close-line"></i>
                </a>
            </span>
            <h3 class="text-center">Thống Kê 12 Tháng Trong Năm {{ year }}</h3>

            <div style="">
                <Componentchart :thongke="thongkeData" />
            </div>
            <div class="text-center"
                style="padding: 5px 10px;display: flex;justify-content: center;align-items: center;">
                <ul v-if="showyear" class="time-options"
                    style="border: 1px solid;border-radius: 5px;padding: 10px; position: absolute;background-color: aliceblue;width: 150px;margin-top: -40px !important;">
                    <li style="display: flex;margin: 5px;" v-for="(hour, index) in years" :key="index"
                        @click.stop="year = hour.value; showyear = !showyear">
                        <input type="checkbox"
                            style="color: aqua;background-color: var(--color-green2);padding-top: 20px;" disabled
                            :checked="hour.value == year">
                        <label style="margin-left: 10px;" for=""> {{ hour.value }}</label>
                    </li>
                </ul>
                <label>Chọn năm thống kê: <label @click="showyear = !showyear"
                        style="padding: 10px 15px;border: 1px solid;border-radius: 5px; margin-right: 45px">{{ year
                        }}</label></label>

                <button class="btn nutshort" @click="updateData">Cập nhật dữ liệu</button>
            </div>
        </div>
    </div>


</template>

<script>
import Componentchart from "./Componentchart.vue";
import {
    Chart,
    BarController,
    BarElement,
    CategoryScale,
    LinearScale,
    Title,
    Tooltip,
    Legend,
} from "chart.js";
import InvoiceService from '../../../../Service/api/InvoiceService';
import { year } from "vue-cal/dist/i18n/ar.es.js";

// Đăng ký các thành phần cần thiết
Chart.register(
    BarController,
    BarElement,
    CategoryScale,
    LinearScale,
    Title,
    Tooltip,
    Legend
);

export default {
    components: {
        Componentchart,
    },
    data() {
        const yearnow = new Date().getFullYear();
        return {
            thongkeDataDemo: [
                {
                    month: 1, revenue: 123000
                },
                {
                    month: 2, revenue: 1234000
                },
                {
                    month: 3, revenue: 345000
                },
                {
                    month: 4, revenue: 456000
                },
                {
                    month: 5, revenue: 567000
                },
                {
                    month: 6, revenue: 1678000
                },
                {
                    month: 7, revenue: 789000
                },
                {
                    month: 8, revenue: 890000
                },
                {
                    month: 9, revenue: 901000
                },
                {
                    month: 10, revenue: 101000
                },
                {
                    month: 11, revenue: 202000
                },
                {
                    month: 12, revenue: 3303000
                },
            ],
            thongkeData:[],
            year: new Date().getFullYear(),
            years: {
                0: { value: (yearnow - 1), name: (yearnow - 1).toString() },
                1: { value: yearnow, name: yearnow.toString() },
                2: { value: (yearnow + 1), name: (yearnow + 1).toString() },
            },
            showyear: false
        };
    },
    methods: {
        async updateData() {
            if (this.year == 2023) {
                this.thongkeData = this.thongkeDataDemo;
            }
            else {
                const response = await InvoiceService.CalculateRevenuesByMonth(this.year);
                console.log("Response: ", response);
                this.thongkeData = response.data;
            }
        },
        Close() {

            window.location.href = 'http://localhost:5173/Information/User/User';
        }
    },
    created() {
        this.updateData();
    },
};
</script>

<style>
.container {
    max-width: 800px;
    margin: auto;
}
</style>