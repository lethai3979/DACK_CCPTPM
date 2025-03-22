<template>

    <h1>Lịch sử giao dịch</h1>

    <div v-for="item in invoices" :key="item.id">
        <hr>
        <div @click="this.$router.push({ name: 'user-invoices-detail', params: { id: item.id } })" v-if="item.refundInvoice == false" class="invoiceB" style="border: 5px solid var(--color-green2);">
            <div class="leftB" style="background-color: var(--color-green2);">
                <img width="60" height="60" src="/src/assets/logoWeb/invoicepayment.png" />
                <div style="color: aliceblue;">Thanh toán</div>
            </div>
            <div class="centerB">
                <div style="font-size: 23px;font-weight: bold;">Tổng tiền: {{ formatMoney(item.total) }}đ</div>
                <div style="font-size: 18px;color: #434343;opacity: 0.8; font-weight: 500; line-height: 16px;">Ngày giao
                    dịch: {{ formatDate(item.createdOn) }}</div>
            </div>
            <div class="rightB">
                <!-- <router-link :to="{ name: 'user-invoices-detail', params: { id: item.id } }">
                    Chi tiết
                </router-link> -->
            </div>
        </div>

        <div @click="this.$router.push({ name: 'user-invoices-detail', params: { id: item.id } })" v-if="item.refundInvoice == true" class="invoiceB" style="border: 5px solid var(--color-red1);">
            <div class="leftB1" style="background-color: #cc0033;">
                <img width="60" height="60" src="/src/assets/logoWeb/invoicerefund1.png" />
                <div style="color: aliceblue;">Hoàn trả</div>
            </div>
            <div class="centerB">
                <div style="font-size: 23px;font-weight: bold;">Tổng tiền: {{ formatMoney(item.total) }}đ</div>
                <div style="font-size: 18px;color: #434343;opacity: 0.8; font-weight: 500; line-height: 16px;">Ngày hoàn trả: {{ formatDate(item.modifiedOn) }}</div>
            </div>
            <div class="rightB">
                <!-- <router-link :to="{ name: 'user-invoices-detail', params: { id: item.id } }">
                    Chi tiết
                </router-link> -->
            </div>
        </div>
    </div>
    <!-- <table class="table">
        <thead>
            <tr>
                <th>
                    Tổng tiền
                </th>
                <th>
                    Ngày giao dịch
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr >
                <td v-if="item.total < 0"> 
                     @{
                        if (item.Total < 0)
                        {
                            @("+" + (-item.Total).ToString())
                        }
                        else
                        {
                            @Html.DisplayFor(modelItem => item.Total)
                        }
                    } 
    
    
                </td>
                <td v-else> 
                dvkbjdv
                </td>
                <td>
                    {{ item.createdOn }}
                    
                </td>
                <td>
                    <a @click="this.$router.push({ name: 'user-invoice-detail', params: { id: item.id } });">Chi tiết hóa đơn</a>
                </td>
            </tr>

        </tbody>
    </table> -->

</template>

<script>
import InvoiceVM from '../../../Model/InvoiceVM';
import InvoiceService from '../../../Service/api/InvoiceService'
export default {
    data() {
        return {
            invoices: [new InvoiceVM()],
        }
    },
    methods: {
        async getAllInvoice() {
            const response = await InvoiceService.getAllInvoicePersonal();
            console.log("Invoice: ", response.data);
            this.invoices = response.data;
        },
        formatDate(isoString) {
            const dateObj = new Date(isoString);

            const hours = dateObj.getHours().toString().padStart(2, '0'); // Lấy giờ
            const minutes = dateObj.getMinutes().toString().padStart(2, '0'); // Lấy phút
            const day = dateObj.getDate().toString().padStart(2, '0'); // Lấy ngày
            const month = (dateObj.getMonth() + 1).toString().padStart(2, '0'); // Lấy tháng (bắt đầu từ 0)
            const year = dateObj.getFullYear(); // Lấy năm

            return `${hours}:${minutes} ${day}/${month}/${year}`;
        },
        formatMoney(value) {
            return new Intl.NumberFormat('vi-VN').format(value);
        },
    },
    created() {
        this.getAllInvoice();
    },

}
</script>

<style>
.invoiceB {
    margin-block: 2px;

    display: flex;
    border-radius: 15px;
    justify-content: space-between;
    align-items: center;
    /* padding: 30px 40px; */
}

.invoiceB .leftB {
    padding: 0px 40px;
    border-top-left-radius: 8px;
    border-bottom-left-radius: 8px;

}

.invoiceB .leftB1 {
    padding: 0px 50px;
    border-top-left-radius: 10px;
    border-bottom-left-radius: 10px;

}

.invoiceB .rightB {
    padding: 30px 40px;
}
</style>