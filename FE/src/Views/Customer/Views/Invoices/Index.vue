<template>

<h1>Lịch sử giao dịch</h1>


<table class="table">
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
        <tr v-for="item in invoices" :key="item.id">
            <td>
                <!-- @{
                    if (item.Total < 0)
                    {
                        @("+" + (-item.Total).ToString())
                    }
                    else
                    {
                        @Html.DisplayFor(modelItem => item.Total)
                    }
                } -->


            </td>
            <td>
                <!-- @Html.DisplayFor(modelItem => item.CreatedOn) -->
            </td>
            <td>
                <!-- <a @click="this.$router.push({ name: 'user-invoice-detail', params: { id: item.id } });">Chi tiết hóa đơn</a> -->
            </td>
        </tr>
<!-- } -->
    </tbody>
</table>

</template>

<script>
import InvoiceVM from '../../../../Model/InvoiceVM';
import InvoiceService from '../../../../Service/api/InvoiceService'
    export default{
  data () {
    return {
        invoices: [new InvoiceVM()],
    }
  },
  methods: {
    async getAllInvoice(){
        const response = await InvoiceService.getAllInvoicePersonal();
        console.log(response);
    }
  },
  created () {
    this.getAllInvoice();
  },

    }
</script>

<style>

</style>
