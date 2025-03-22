<template>
    <div class="report Login">

        <h4>Report bài viết</h4>
        <hr />
        <div class="w18">
            <div class="">
                <span class="icon-closeForm">
                    <a class="custom-link" @click="close()">
                        <i class="ri-close-line"></i>
                    </a>
                </span>
                <Message ref="message" />
                <form @submit.prevent="Create">
                    <div class="form-group">
                        <label class="control-label">Nội dung report</label>
                        <input v-model="reportDto.content" class="form-control" required />
                        <span asp-validation-for="Content" class="text-danger"></span>
                    </div>
                    <div class="form-group">
                        <div class="dropdown" @click="toggleDropdown">
                            <div class="selected-option">{{ selectedOption ? selectedOption.name : 'Chọn loại report' }}
                            </div>
                            <ul v-if="dropdownVisible" class="dropdown-list">
                                <li v-for="item in reportType" :key="item.id" @click.stop="selectOption(item)">
                                    {{ item.name }}
                                </li>
                            </ul>
                        </div>
                    </div>

                    <div class="form-group">
                        <input type="submit" value="Create" class="btn btn-primary" />
                    </div>
                </form>
            </div>
        </div>

    </div>
</template>

<script>
import ReportDTO from '../../../../DTOs/ReportDto';
import ReportTypeService from '../../../../Service/api/ReportTypeService';
import Message from '../../../../Message.vue';
import ReportService from '../../../../Service/api/ReportService';
export default {
    props: {
        id: {
            type: Number,
            default: 0,
            required: true
        },
    },
    components: {
        Message
    },
    data() {
        return {
            reportType: null,
            reportDto: new ReportDTO(),
            selectedOption: null,
            dropdownVisible: false
        }
    },
    methods: {
        async Create() {
            console.log(this.reportDto);
            if (this.reportDto.reportTypeId == 0 || this.reportDto.content == '') {
                this.open("Vui lòng điền đầy đủ thông tin");
            }
            else {
                this.reportDto.postId = this.id;
                console.log("reportDto: ", this.reportDto);
                const response = await ReportService.AddReport(this.reportDto);
                if (response.success) {
                    this.close();
                }
            }
        },
        async getAllReportType() {
            const response = await ReportTypeService.GetAll();
            console.log("ReportType by User: ", response);
            this.reportType = response.data;
        },
        toggleDropdown() {
            this.dropdownVisible = !this.dropdownVisible;
        },
        selectOption(item) {
            this.reportDto.reportTypeId = item.id;
            this.selectedOption = item;
            this.toggleDropdown();
        },
        open(message) {
            this.$refs.message.ShowMess(message);
        },
        close() {
            this.$emit('Close');
        },
    },
    async created() {
        await this.getAllReportType()
    },

}
</script>

<style>
.report{
    position: relative;
    width: 500px !important;
    height: 400px !important;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
    padding: auto;
    align-content: center;
    justify-content: center;
    justify-items: center;
    border-radius: 15px;
}
.w18{
    width: 80%;
    }
.dropdown {
    position: relative;
    cursor: pointer;
    border: 1px solid #ccc;
    padding: 10px;
}

.selected-option {
    background-color: #f9f9f9;
}

.dropdown-list {
    list-style: none;
    margin: 0;
    padding: 0;
    border: 1px solid #ccc;
    max-height: 150px;
    overflow-y: auto;
    position: absolute;
    width: 100%;
    background-color: white;
    z-index: 1000;
}

.dropdown-list li {
    padding: 10px;
    cursor: pointer;
}

.dropdown-list li:hover {
    background-color: #f0f0f0;
}
</style>