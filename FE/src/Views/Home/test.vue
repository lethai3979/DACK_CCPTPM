<style>
.time-choose {
    display: flex;
    grid-gap: 8px;
    gap: 8px;
    align-items: center;
    width: 100%;
}

.time-choose__item {
    position: relative;
    border: 1px solid #e0e0e0;
    width: 100%;
    cursor: pointer;
    padding: 8px 16px;
    border-radius: 8px;
    background-color: #fff;
}

.active-time {
    border: none;
    background-color: #fff;
    padding: auto;
    width: 100%;
}

.active-time-dropdown {
    position: relative;
    width: 100%;
}

.selected-time {
    /* padding: 10px; */
    border: none;
    cursor: pointer;
    width: 100%;
    display: flex;
    justify-content: space-between;
}

.time-options {
    position: absolute;
    width: 100%;
    background-color: white;
    border: 1px solid #ccc;
    margin: 0;
    padding: 0;
    list-style-type: none;
    z-index: 10;
    height: 180px;
    overflow-y: auto;
}

.time-options li {
    padding: 10px;
    cursor: pointer;
}

.time-options li.disabled {
    color: gray;
    cursor: not-allowed;
}


.container_lich {
    position: relative;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
    padding: auto;
    align-content: center;
    justify-content: center;
    justify-items: center;
    width: 700px;
    height: 82%;
    border-radius: 15px;
}

.container_lich .tren {
    padding: 20px;
}

.container_lich .tren button {
    padding: 7px 15px;

}

.container_lich .tren1 {
    padding: 10px;
}

ul {
    padding: 0 !important;
    margin: 0 !important;
    margin-left: 0 !important;
}
</style>
<template>
    <Message ref="message" />
    <div class="container_lich Login">
        <!-- <button @click="close">X</button> -->
        <span class="icon-closeForm">
            <a class="custom-link" @click="close()">
                <i class="ri-close-line"></i>
            </a>
        </span>
        <h3 style="margin-bottom: 0px !important;" class="tren1">Thời gian</h3>
        <ul>
            <li>
                <vue-cal class="lichvue" style="margin-bottom: 20px;height: 380px ;max-width: 650px;"
                    :default-view="'month'" :disable-views="['years', 'year', 'week', 'day']"
                    :selected-date="currentStartDate" :min-date="getFirstDayOfCurrentMonth()"
                    :disable-days="disabledDates" :months-to-show="1" @cell-click="onDateSelect"
                    @cell-mouseenter="onCellMouseEnter" :cell-class="(cell) => getCellClasses(cell)"
                    @cell-render="customCellRenderer" />
                <!-- :on-cell-render="customCellRenderer" -->
            </li>
            <li>
                <div class="time-choose ">
                    <div class="time-choose__item" @click="toggleDropdown">
                        <div>
                            <p class="title-time">Nhận xe</p>
                            <div class="active-time-dropdown">
                                <div class="selected-time">
                                    <label for="">{{ selectedStartTime !== null ? availableHours[selectedStartTime].name
                                        :
                                        'Chọn giờ bắt đầu' }}</label>
                                    <img v-if="!dropdownOpen" width="24" height="24"
                                        src="https://img.icons8.com/ios/50/expand-arrow--v2.png"
                                        alt="expand-arrow--v2" />
                                    <img v-if="dropdownOpen" width="24" height="24"
                                        src="https://img.icons8.com/ios/50/collapse-arrow--v2.png"
                                        alt="collapse-arrow--v2" />
                                </div>
                                <!-- Danh sách các tùy chọn -->
                                <ul v-if="dropdownOpen != false" class="time-options"
                                    style="margin-top: -235px !important;">
                                    <li style="display: flex;" v-for="(hour, index) in availableHours" :key="index"
                                        @click.stop="selectTime(index)" :class="{ disabled: hour.disabled }"
                                        :disabled="hour.disabled" :hidden="startDate == null">
                                        <input type="checkbox"
                                            style="color: aqua;background-color: var(--color-green2);padding-top: 20px;"
                                            :checked="hour.value == selectedStartTime">
                                        <label style="margin-left: 10px;" for=""> {{ hour.name }}</label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="time-choose__item" @click="toggleDropdownV2">
                        <div>
                            <p class="title-time">Trả xe</p>
                            <div class="active-time-dropdown">
                                <!-- Hiển thị giá trị đã chọn -->
                                <div class="selected-time">
                                    <label for="">{{ selectedEndTime !== null ? availableHours[selectedEndTime].name
                                        :
                                        'Chọn giờ kết thúc' }}</label>
                                    <img v-if="!dropdownOpenV2" width="24" height="24"
                                        src="https://img.icons8.com/ios/50/expand-arrow--v2.png"
                                        alt="expand-arrow--v2" />
                                    <img v-if="dropdownOpenV2" width="24" height="24"
                                        src="https://img.icons8.com/ios/50/collapse-arrow--v2.png"
                                        alt="collapse-arrow--v2" />
                                </div>
                                <!-- Danh sách các tùy chọn -->
                                <ul v-if="dropdownOpenV2" class="time-options" style="margin-top: -235px !important;">
                                    <li style="display: flex;" v-for="(hour, index) in availableHoursV2" :key="index"
                                        @click.stop="selectTimeV2(index)" :class="{ disabled: hour.disabled }"
                                        :disabled="hour.disabled" :hidden="startDate == null">
                                        <input type="checkbox"
                                            style="color: aqua;background-color: var(--color-green2);padding-top: 20px;"
                                            :checked="hour.value == selectedEndTime">
                                        <label style="margin-left: 10px;" for=""> {{ hour.name }}</label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </li>
            <!-- <label>Thời gian nhận và trả thuê xe: {{ selectedStartTime }} - {{ selectedEndTime }}</label> -->
            <div style="display: flex;justify-content: space-between;" class="tren">
                <label>Ngày thuê xe: {{ selectedStartTime }}:00, {{ startDate || 'Chưa chọn' }} - {{ selectedEndTime
                    }}:00, {{ endDate || 'Chưa chọn' }}</label>
                <button class="btn nutshort" :disabled="selectedEndTime == null || endDate == null"
                    @click="Submit()">Tiếp tục</button>
            </div>
        </ul>
    </div>
</template>
<script>
import VueCal from 'vue-cal';
import 'vue-cal/dist/vuecal.css';
import Message from '../../Message.vue';
import BookingService from '../../Service/api/BookingService';
export default {
    name: 'BookingCalendar',
    props: {
        id: {
            type: Number,
            default: 0,
        },
    },
    components: {
        VueCal, Message
    },
    data() {
        return {
            dropdownOpen: false,
            dropdownOpenV2: false,
            show: false,
            message: "",
            booking: {
            },
            availableHours: {
                0: { value: 0, name: "00:00", disabled: false },
                1: { value: 1, name: "01:00", disabled: false },
                2: { value: 2, name: "02:00", disabled: false },
                3: { value: 3, name: "03:00", disabled: false },
                4: { value: 4, name: "04:00", disabled: false },
                5: { value: 5, name: "05:00", disabled: false },
                6: { value: 6, name: "06:00", disabled: false },
                7: { value: 7, name: "07:00", disabled: false },
                8: { value: 8, name: "08:00", disabled: false },
                9: { value: 9, name: "09:00", disabled: false },
                10: { value: 10, name: "10:00", disabled: false },
                11: { value: 11, name: "11:00", disabled: false },
                12: { value: 12, name: "12:00", disabled: false },
                13: { value: 13, name: "13:00", disabled: false },
                14: { value: 14, name: "14:00", disabled: false },
                15: { value: 15, name: "15:00", disabled: false },
                16: { value: 16, name: "16:00", disabled: false },
                17: { value: 17, name: "17:00", disabled: false },
                18: { value: 18, name: "18:00", disabled: false },
                19: { value: 19, name: "19:00", disabled: false },
                20: { value: 20, name: "20:00", disabled: false },
                21: { value: 21, name: "21:00", disabled: false },
                22: { value: 22, name: "22:00", disabled: false },
                23: { value: 23, name: "23:00", disabled: false },
            },
            availableHoursV2: {
                0: { value: 0, name: "00:00", disabled: false },
                1: { value: 1, name: "01:00", disabled: false },
                2: { value: 2, name: "02:00", disabled: false },
                3: { value: 3, name: "03:00", disabled: false },
                4: { value: 4, name: "04:00", disabled: false },
                5: { value: 5, name: "05:00", disabled: false },
                6: { value: 6, name: "06:00", disabled: false },
                7: { value: 7, name: "07:00", disabled: false },
                8: { value: 8, name: "08:00", disabled: false },
                9: { value: 9, name: "09:00", disabled: false },
                10: { value: 10, name: "10:00", disabled: false },
                11: { value: 11, name: "11:00", disabled: false },
                12: { value: 12, name: "12:00", disabled: false },
                13: { value: 13, name: "13:00", disabled: false },
                14: { value: 14, name: "14:00", disabled: false },
                15: { value: 15, name: "15:00", disabled: false },
                16: { value: 16, name: "16:00", disabled: false },
                17: { value: 17, name: "17:00", disabled: false },
                18: { value: 18, name: "18:00", disabled: false },
                19: { value: 19, name: "19:00", disabled: false },
                20: { value: 20, name: "20:00", disabled: false },
                21: { value: 21, name: "21:00", disabled: false },
                22: { value: 22, name: "22:00", disabled: false },
                23: { value: 23, name: "23:00", disabled: false },
            },
            result: {
                key: 0,
                value: null
            },
            startDate: null,
            endDate: null,
            selectedStartTime: null,
            selectedEndTime: null,
            selectedDates: [],
            currentStartDate: new Date(),
            isFirstClick: true,
            hoveredDate: null, // Thêm biến để theo dõi ngày đang hover
        };
    },
    computed: {

        disabledDates() {
            let disabled = [];
            // const disabled = [];
            const bookingDates = {}; // Object để đếm số lần xuất hiện của mỗi ngày
            const today = new Date();
            today.setHours(0, 0, 0, 0);

            // Thêm các ngày đã qua

            // Đếm số lần xuất hiện của mỗi ngày trong các booking
            Object.values(this.booking).forEach(({ startLimit, endLimit }) => {
                let bookingStartDate = new Date(startLimit);
                let bookingEndDate = new Date(endLimit);

                // Format ngày để sử dụng làm key
                const startDateKey = bookingStartDate.format();
                const endDateKey = bookingEndDate.format();

                // Tăng số đếm cho ngày bắt đầu và kết thúc
                bookingDates[startDateKey] = (bookingDates[startDateKey] || 0) + 1;
                bookingDates[endDateKey] = (bookingDates[endDateKey] || 0) + 1;

                // Xử lý các ngày nằm giữa startLimit và endLimit
                if (bookingStartDate.getTime() !== bookingEndDate.getTime()) {
                    let currentDay = new Date(bookingStartDate);
                    currentDay.setDate(currentDay.getDate() + 1);

                    while (currentDay < bookingEndDate) {
                        const currentDayKey = currentDay.format();
                        disabled.push(currentDayKey);
                        currentDay.setDate(currentDay.getDate() + 1);
                    }
                }
            });

            // Xử lý logic khóa ngày dựa trên giờ và số lần xuất hiện
            Object.values(this.booking).forEach(({ startLimit, endLimit }) => {
                let bookingStartDate = new Date(startLimit);
                let bookingEndDate = new Date(endLimit);

                const startDateKey = bookingStartDate.format();
                const endDateKey = bookingEndDate.format();

                // Nếu ngày xuất hiện trong 2 booking trở lên, luôn khóa
                if (bookingDates[startDateKey] >= 2) {
                    if (!disabled.includes(startDateKey)) {
                        disabled.push(startDateKey);
                    }
                } else {
                    // Nếu chỉ xuất hiện 1 lần, áp dụng logic giờ cũ
                    const bookingStartHour = bookingStartDate.getHours();
                    if (bookingStartHour < 12) {
                        disabled.push(startDateKey);
                    }
                }

                if (bookingDates[endDateKey] >= 2) {
                    if (!disabled.includes(endDateKey)) {
                        disabled.push(endDateKey);
                    }
                } else {
                    const bookingEndHour = bookingEndDate.getHours();
                    if (bookingEndHour >= 12) {
                        disabled.push(endDateKey);
                    }
                    else {

                        disabled = disabled.filter(date => date !== endDateKey);
                    }
                }
            });
            let currentDate = new Date(today);
            currentDate.setDate(1);
            while (currentDate < today) {
                disabled.push(new Date(currentDate).format());
                currentDate.setDate(currentDate.getDate() + 1);
            }

            // Loại bỏ các ngày trùng lặp
            return [...new Set(disabled)];
        }

    },


    methods: {
        Submit() {
            console.log(this.selectedStartTime, this.selectedEndTime);
            const startday = this.formatDateTime(this.startDate, this.selectedStartTime);
            const endday = this.formatDateTime(this.endDate, this.selectedEndTime);
            this.$emit("GetDay", startday, endday);
        },
        async fecthHourByBooking() {
            try {
                const response = await PromotionService.getPromotion(id);
                if (response && response.data) {
                    this.promotion = response.data;
                } else {
                    console.warn("No promotion data received");
                }
            } catch (error) {
                console.error("Error fetching promotion:", error);
                throw error; // Re-throw để có thể bắt ở UserVoucher
            }
        },
        formatDateTime(date, time) {
            if (!date || !time) return null;
            if (time < 10) {
                return `${date}T0${time}:00`;
            }
            else {
                return `${date}T${time}:00`;
            }
        },

        getFirstDayOfCurrentMonth() {
            const today = new Date();
            // Tạo ngày đầu tiên của tháng hiện tại
            return new Date(today.getFullYear(), today.getMonth(), 1);
        },
        open(message) {
            this.$refs.message.ShowMess(message);
        },
        close() {
            //this.show = false;
            this.$emit('Close');
        },

        toggleDropdown() {
            if (this.startDate != null) {
                this.dropdownOpen = !this.dropdownOpen;
            }
            this.CheckHours();
        },
        selectTime(index) {
            // Kiểm tra nếu tùy chọn không bị khóa
            if (!this.availableHours[index].disabled) {
                this.selectedStartTime = index;
                this.dropdownOpen = false;
                console.log("Dropdown should now be closed:", this.dropdownOpen);
            }
        },
        toggleDropdownV2() {
            if (this.endDate != null && this.selectedStartTime != null) {
                this.dropdownOpenV2 = !this.dropdownOpenV2;
                console.log(this.dropdownOpenV2);
            } this.CheckHoursV2();
        },
        selectTimeV2(index) {
            // Kiểm tra nếu tùy chọn không bị khóa
            if (!this.availableHoursV2[index].disabled) {
                this.selectedEndTime = index;
                console.log("Index: ", index);
                this.dropdownOpenV2 = false; // Đóng dropdown sau khi chọn
                // Gọi hàm CheckHours sau khi chọn giá trị
            }
        },


        CheckHours() {
            if (this.startDate == null) { this.open("Vui lòng chọn ngày bắt đầu!"); }
            if (this.startDate != null) {
                const formattedNow = new Date(this.startDate);
                const formattedNowDateString = this.formatDateToLocal(formattedNow);
                const todaynow = new Date();
                const today = this.formatDateToLocal(todaynow);
                // const hourtoday = todaynow.
                let isBooked = false; // Flag để kiểm tra ngày có nằm trong booking không

                // Đầu tiên reset tất cả các giờ về trạng thái false
                Object.values(this.availableHours).forEach(hour => {
                    hour.disabled = false;
                });

                // Lặp qua các booking để kiểm tra
                Object.values(this.booking).forEach(e => {
                    let bookingStartDate = new Date(e.startLimit);
                    let bookingEndDate = new Date(e.endLimit);
                    const bookingStartHour = bookingStartDate.getHours();
                    const bookingEndHour = bookingEndDate.getHours();
                    const bookingStartDateString = this.formatDateToLocal(bookingStartDate);
                    const bookingEndDateString = this.formatDateToLocal(bookingEndDate);
                    if (today === formattedNowDateString) {
                        // Nếu trùng ngày bắt đầu booking
                        console.log("CheckHours điều kiện 1");
                        isBooked = true;
                        this.resultHour('today', todaynow.getHours());
                        return;
                    }
                    if (bookingStartDateString === formattedNowDateString) {
                        // Nếu trùng ngày bắt đầu booking
                        console.log("CheckHours điều kiện 2");
                        isBooked = true;
                        this.resultHour('start', bookingStartHour);
                        return;
                    }
                    else if (bookingEndDateString === formattedNowDateString) {
                        // Nếu trùng ngày kết thúc booking
                        console.log("CheckHours điều kiện 3");
                        isBooked = true;
                        this.resultHour('end', bookingEndHour);
                        return;
                    }
                    else if (formattedNow > bookingStartDate && formattedNow < bookingEndDate) {
                        // Nếu nằm giữa khoảng booking
                        console.log("CheckHours điều kiện 4");
                        isBooked = true;
                        this.resultHour('middle', 0); // Khóa tất cả các giờ
                        return;
                    }
                });

                if (!isBooked) {
                    // Nếu là ngày trống, mở tất cả các giờ
                    this.resultHour('free', 0);
                }
            }
        },

        CheckHoursV2() {
            if (this.endDate == null) {
                this.open("Vui lòng chọn ngày kết thúc!");
            }
            if (this.selectedStartTime == null) {
                this.open("Vui lòng chọn giờ ngày bắt đầu!");
            }
            if (this.endDate != null && this.selectedStartTime != null) {
                const formattedNow = new Date(this.endDate);
                const formattedStart = new Date(this.startDate);
                const formattedNowDateString = this.formatDateToLocal(formattedNow);
                const formattedStartDateString = this.formatDateToLocal(formattedStart);
                const todaynow = new Date();
                const today = this.formatDateToLocal(todaynow);
                let isBooked = false; // Flag để kiểm tra ngày có nằm trong booking không

                // Đầu tiên reset tất cả các giờ về trạng thái false
                Object.values(this.availableHoursV2).forEach(hour => {
                    hour.disabled = false;
                });

                // Lặp qua các booking để kiểm tra
                Object.values(this.booking).forEach(e => {
                    let bookingStartDate = new Date(e.startLimit);
                    let bookingEndDate = new Date(e.endLimit);

                    const bookingStartHour = bookingStartDate.getHours();
                    const bookingEndHour = bookingEndDate.getHours();

                    const bookingStartDateString = this.formatDateToLocal(bookingStartDate);
                    const bookingEndDateString = this.formatDateToLocal(bookingEndDate);

                    // Nếu trùng ngày bắt đầu booking
                    if (today === formattedNowDateString && formattedStartDateString) {
                        console.log('1');
                        isBooked = true;
                        this.resultHourV2('today&start', todaynow.getHours(), this.selectedStartTime);
                        return;
                    }
                    // Nếu trùng ngày bắt đầu và ngày kết thúc booking

                    // Nếu trùng ngày bắt đầu booking và trùng với giờ bắt đầu
                    else if (bookingStartDateString === formattedNowDateString && bookingStartDateString === formattedStartDateString && isBooked == false) {
                        console.log('2', bookingStartDateString, formattedNowDateString, formattedStartDateString);
                        isBooked = true;
                        this.resultHourV2('start&end&bookingstart', bookingStartHour, this.selectedStartTime);
                        return;
                    }
                    // Nếu trùng ngày bắt đầu booking và trùng với giờ kết thúc
                    else if (bookingEndDateString === formattedNowDateString && bookingEndDateString === formattedStartDateString && isBooked == false) {
                        console.log('3');
                        isBooked = true;
                        this.resultHourV2('start&end&bookingend', bookingEndHour, this.selectedStartTime);
                        return;
                    }
                    // Nếu trùng ngày bắt đầu booking
                    else if (bookingStartDateString === formattedNowDateString && isBooked == false) {
                        console.log('4');
                        isBooked = true;
                        this.resultHourV2('start', bookingStartHour, 0);
                        return;
                    }
                    // Nếu trùng ngày kết thúc booking
                    else if (bookingEndDateString === formattedNowDateString && isBooked == false) {
                        console.log('5');
                        isBooked = true;
                        this.resultHourV2('end', bookingEndHour, 0);
                        return;
                    }

                    else if (formattedNowDateString === formattedStartDateString && bookingEndDateString !== formattedNowDateString && bookingStartDateString !== formattedNowDateString && isBooked == false) {
                        console.log('6', "giờ bắt đầu: ", this.selectedStartTime);
                        isBooked = true;
                        this.resultHourV2('start&end', 0, this.selectedStartTime);
                        return;
                    }
                    // Nếu nằm giữa khoảng booking
                    else if (formattedNow > bookingStartDate && formattedNow < bookingEndDate && isBooked == false) {
                        console.log('7');
                        isBooked = true;
                        this.resultHourV2('middle', 0, 0); // Khóa tất cả các giờ
                        return;
                    }
                });

                if (!isBooked) {
                    // Nếu là ngày trống, mở tất cả các giờ
                    this.resultHourV2('free', 0);
                }
            }
        },


        resultHour(type, hour, hs) {
            const start = parseInt(hs);
            switch (type) {
                case 'start':
                    // Nếu là ngày bắt đầu booking, mở các giờ từ 0h đến giờ bắt đầu booking
                    Object.values(this.availableHours).forEach(p => {
                        p.disabled = p.value >= 12; // Khóa các giờ từ giờ bắt đầu trở đi
                    });
                    break;
                case 'end':
                    // Nếu là ngày kết thúc booking, mở các giờ sau giờ kết thúc booking
                    Object.values(this.availableHours).forEach(p => {
                        p.disabled = p.value <= 12; // Khóa các giờ từ 0h đến giờ kết thúc
                    });
                    break;
                case 'middle':
                    // Nếu nằm giữa khoảng booking, khóa tất cả các giờ
                    Object.values(this.availableHours).forEach(p => {
                        p.disabled = true;
                    });
                    break;
                case 'free':
                    // Nếu là ngày trống, mở tất cả các giờ
                    Object.values(this.availableHours).forEach(p => {
                        p.disabled = false;
                    });
                    break;
            }
        },
        resultHourV2(type, hour, hs) {
            const start = parseInt(hs);
            switch (type) {
                case 'today&start':
                    // Nếu là ngày bắt đầu booking, mở các giờ từ 0h đến giờ bắt đầu booking
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = p.value < start + 2;  // Khóa các giờ từ giờ bắt đầu trở đi
                    });
                    break;
                case 'start&end':
                    // Nếu ngày bắt đầu = ngày kết thúc, khóa các giờ từ 0h đến giờ bắt đầu
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = p.value < start + 1;  // Khóa các giờ từ giờ bắt đầu trở đi
                    });
                    break;
                case 'start&end&bookingstart':
                    console.log(2);
                    // Nếu ngày bắt đầu = ngày kết thúc và trùng với giờ bắt đầu, khóa các giờ từ 0h đến giờ bắt đầu
                    Object.values(this.availableHoursV2).forEach(p => {
                        //p.disabled = (p.value < 2) || (p.value >= 12); 
                        p.disabled = (p.value < start + 1) || (p.value >= 12);
                        //p.disabled = p.value < start + 2;   // Khóa các giờ từ giờ bắt đầu trở đi
                    });

                    break;
                case 'start&end&bookingend':
                    // Nếu ngày bắt đầu = ngày kết thúc và trùng với giờ kết thúc, khóa các giờ từ 0h đến giờ kết thúc
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = (p.value < start + 1) || (p.value <= 12);
                        // p.disabled = p.value < start + 2;  // Khóa các giờ từ giờ bắt đầu trở đi
                    });
                    break;
                case 'today':
                    // Nếu là ngày bắt đầu booking, mở các giờ từ 0h đến giờ bắt đầu booking
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = p.value < hour + 2; // Khóa các giờ từ giờ bắt đầu trở đi
                    });
                    break;
                case 'start':
                    // Nếu là ngày bắt đầu booking, mở các giờ từ 0h đến giờ bắt đầu booking
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = p.value >= 12; // Khóa các giờ từ giờ bắt đầu trở đi
                    });
                    console.log("Vaof ddaay ngafy 29 taay");
                    break;
                case 'end':
                    // Nếu là ngày kết thúc booking, mở các giờ sau giờ kết thúc booking
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = p.value <= 12; // Khóa các giờ từ 0h đến giờ kết thúc
                    });
                    break;
                case 'middle':
                    // Nếu nằm giữa khoảng booking, khóa tất cả các giờ
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = true;
                    });
                    break;
                case 'free':
                    // Nếu là ngày trống, mở tất cả các giờ
                    Object.values(this.availableHoursV2).forEach(p => {
                        p.disabled = false;
                    });
                    break;
            }
        },
        formatDateToLocal(date) {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0
            const day = String(date.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        },

        hasBookedDatesInRange(startDate, endDate) {
            const start = new Date(startDate);
            const end = new Date(endDate);

            // Kiểm tra xem start và end có cùng ngày không (bỏ qua giờ)
            const isSameDay = (date1, date2) => {
                return date1.getFullYear() === date2.getFullYear() &&
                    date1.getMonth() === date2.getMonth() &&
                    date1.getDate() === date2.getDate();
            };

            // Biến để kiểm tra xem có booking nào trùng không
            let hasOverlap = false;

            for (const { startLimit, endLimit } of Object.values(this.booking)) {
                const bookingStart = new Date(startLimit);
                const bookingEnd = new Date(endLimit);
                const startHour = bookingStart.getHours();
                const endHour = bookingEnd.getHours();

                console.log("Start Hour:", startHour, "End Hour:", endHour);

                // Kiểm tra nếu ngày bắt đầu và kết thúc nằm trong khoảng booking
                if (
                    (start <= bookingEnd && start >= bookingStart) ||
                    (end <= bookingEnd && end >= bookingStart) ||
                    (start <= bookingStart && end >= bookingEnd)
                ) {
                    hasOverlap = true; // Đánh dấu là có trùng

                    // Trường hợp 1: Start và End cùng ngày
                    if (isSameDay(start, end)) {
                        if (isSameDay(start, bookingStart)) {
                            // Nếu trùng ngày bắt đầu của booking
                            return startHour < 12; // false nếu startHour > 12
                        }
                        if (isSameDay(start, bookingEnd)) {
                            // Nếu trùng ngày kết thúc của booking
                            return endHour > 12; // false nếu endHour < 12
                        }
                        return true;
                    }

                    // Trường hợp 2: Ngày bắt đầu trùng với ngày kết thúc của booking
                    if (isSameDay(start, bookingEnd)) {
                        if (endHour < 12) {
                            console.log("B2: Ngày bắt đầu trùng ngày kết thúc booking, giờ kết thúc < 12");
                            return false;
                        }
                    }

                    // Trường hợp 3: Ngày kết thúc trùng với ngày bắt đầu của booking
                    if (isSameDay(end, bookingStart)) {
                        if (startHour > 12) {
                            console.log("B3: Ngày kết thúc trùng ngày bắt đầu booking, giờ bắt đầu > 12");
                            return false;
                        }
                    }
                }
            }

            // Nếu không có booking nào trùng thì return false
            if (!hasOverlap) {
                console.log("Không có booking nào trùng");
                return false;
            }

            // Nếu có trùng nhưng không rơi vào các trường hợp đặc biệt thì return true
            return true;
        },

        onDateSelect(cellData) {
            const selectedDate = cellData.format();
            this.dropdownOpen = this.dropdownOpenV2 = false;
            this.selectedStartTime = this.selectedEndTime = null;
            if (this.isFirstClick) {
                // Click đầu tiên
                this.startDate = selectedDate;
                console.log(this.startDate);
                this.endDate = null;
                this.isFirstClick = false;

            } else {
                // Click thứ hai
                const firstDate = new Date(this.startDate);
                const secondDate = new Date(selectedDate);

                if (this.hasBookedDatesInRange(
                    firstDate < secondDate ? firstDate : secondDate,
                    firstDate < secondDate ? secondDate : firstDate
                )) {
                    alert('Không thể chọn khoảng thời gian này vì có ngày đã được đặt. Vui lòng chọn lại.');
                    this.isFirstClick = true;
                    this.startDate = null;
                    this.endDate = null;
                } else {
                    if (firstDate < secondDate) {
                        this.endDate = selectedDate;
                    } else {
                        this.endDate = this.startDate;
                        this.startDate = selectedDate;
                    }
                    this.isFirstClick = true;
                }
            }
            this.hoveredDate = null; // Reset hoveredDate sau mỗi lần click
        },

        // onDateSelect(cellData) {
        //     const selectedDate = cellData.format();  // Đảm bảo rằng cellData.format() trả về định dạng hợp lệ
        //     const dateObject = new Date(selectedDate);

        //     // Lấy ngày, tháng và năm trong đối tượng Date
        //     const day = dateObject.getDate();
        //     const month = dateObject.getMonth(); // Lấy tháng (0 - 11)
        //     const year = dateObject.getFullYear(); // Lấy năm

        //     const currentDate = new Date();
        //     const currentMonth = currentDate.getMonth();
        //     const currentYear = currentDate.getFullYear();

        //     // Kiểm tra xem ngày đã chọn có phải là trong tháng và năm hiện tại không
        //     if (month === currentMonth && year === currentYear) {
        //         // Các thao tác khác với startDate và endDate
        //         this.dropdownOpen = this.dropdownOpenV2 = false;
        //         this.selectedStartTime = this.selectedEndTime = null;

        //         if (this.isFirstClick) {
        //             // Click đầu tiên: Chọn ngày bắt đầu
        //             this.startDate = selectedDate;
        //             this.endDate = null;
        //             this.isFirstClick = false;

        //             console.log("Selected Date:", selectedDate);
        //             // Tìm phần tử cell trong DOM bằng selectedDate (sử dụng 'aria-label')
        //             const cell = document.querySelector(`[aria-label="${day}"]`);

        //             if (cell) {
        //                 console.log("Found the cell:", cell);

        //                 // Xóa lớp selected của cell trước đó nếu có
        //                 const previousSelectedCell = document.querySelector('.vuecal__cell--selected');
        //                 if (previousSelectedCell) {
        //                     previousSelectedCell.classList.remove('vuecal__cell--selected');
        //                 }

        //                 // Thêm lớp selected vào cell vừa click
        //                 cell.classList.add('vuecal__cell--selected');
        //             } else {
        //                 console.error("Cell not found for date:", selectedDate);
        //             }

        //         } else {
        //             // Click thứ hai: Chọn ngày kết thúc
        //             const firstDate = new Date(this.startDate);
        //             const secondDate = new Date(selectedDate);

        //             console.log("Selected Date2:", selectedDate);

        //             // Tìm phần tử cell trong DOM bằng selectedDate (sử dụng 'aria-label')
        //             const cell = document.querySelector(`[aria-label="${day}"]`);

        //             if (cell) {
        //                 console.log("Found the cell2:", cell);

        //                 // Xóa lớp selected1 của cell trước đó nếu có
        //                 const previousSelectedCell = document.querySelector('.vuecal__cell--selected1');
        //                 if (previousSelectedCell) {
        //                     previousSelectedCell.classList.remove('vuecal__cell--selected1');
        //                 }

        //                 // Kiểm tra và hoán đổi các lớp nếu cần
        //                 if (firstDate < secondDate) {
        //                     // Ngày bắt đầu (startDate) nhỏ hơn ngày kết thúc (selectedDate)
        //                     cell.classList.add('vuecal__cell--selected1'); // Thêm selected1 vào ngày kết thúc
        //                     const startCell = document.querySelector(`[aria-label="${this.startDate}"]`);
        //                     if (startCell) {
        //                         startCell.classList.add('vuecal__cell--selected'); // Thêm selected vào ngày bắt đầu
        //                     }
        //                 } else {
        //                     // Ngày kết thúc (selectedDate) nhỏ hơn ngày bắt đầu (startDate)
        //                     cell.classList.add('vuecal__cell--selected'); // Thêm selected vào ngày kết thúc
        //                     const startCell = document.querySelector(`[aria-label="${this.startDate}"]`);
        //                     if (startCell) {
        //                         startCell.classList.add('vuecal__cell--selected1'); // Thêm selected1 vào ngày bắt đầu
        //                     }
        //                 }
        //             } else {
        //                 console.error("Cell not found for date:", selectedDate);
        //             }

        //             // Kiểm tra khoảng thời gian đã chọn
        //             const startDateObj = firstDate < secondDate ? firstDate : secondDate;
        //             const endDateObj = firstDate < secondDate ? secondDate : firstDate;

        //             if (this.hasBookedDatesInRange(startDateObj, endDateObj)) {
        //                 alert('Không thể chọn khoảng thời gian này vì có ngày đã được đặt. Vui lòng chọn lại.');
        //                 this.isFirstClick = true;
        //                 this.startDate = null;
        //                 this.endDate = null;
        //             } else {
        //                 // Gán ngày kết thúc hoặc điều chỉnh lại startDate và endDate
        //                 if (firstDate < secondDate) {
        //                     this.endDate = selectedDate;
        //                 } else {
        //                     this.endDate = this.startDate;
        //                     this.startDate = selectedDate;
        //                 }
        //                 this.isFirstClick = true;
        //             }
        //         }

        //         this.hoveredDate = null;
        //     } else {
        //         console.log("Chỉ có thể chọn ngày trong tháng hiện tại.");
        //     }
        // },







        // Thêm method xử lý hover
        onCellMouseEnter(cell) {
            if (!this.isFirstClick) {
                this.hoveredDate = cell.date;
            }
        },
        async getHours() {
            console.log("ID bài post trong file Test: ", this.id);
            const response = await BookingService.GetBookingDays(this.id);
            console.log("Booking days: ", response);
            if (response.success) {
                for (let i = 0; i < response.data.length; i += 2) {
                    this.booking[(i / 2) + 1] = {
                        // startLimit: response.data[i],
                        // endLimit: response.data[i + 1]
                        startLimit: this.convertToCustomFormat(response.data[i]),
                        endLimit: this.convertToCustomFormat(response.data[i + 1])
                    };
                }
            }
            console.log("this.booking: ", this.booking);
        },
        convertToCustomFormat(isoString) {
            let date = new Date(isoString);
            let year = date.getFullYear();
            let month = String(date.getMonth() + 1).padStart(2, '0');
            let day = String(date.getDate()).padStart(2, '0');
            let hours = String(date.getHours()).padStart(2, '0');
            let minutes = String(date.getMinutes()).padStart(2, '0');
            let seconds = String(date.getSeconds()).padStart(2, '0');
            return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
        },
        // customCellRenderer({ cell, date }) {
        //     console.log("Cell: ", cell, "Date: ", date);
        //     console.log("Start Date: ", this.startDate);
        //     console.log("End Date: ", this.endDate);
        //     console.log("Date: ", date);
        //     // Xóa các lớp CSS cũ
        //     cell.className = cell.className.replace(/(start-date|end-date|in-range)/g, '');

        //     // Thêm lớp CSS cho ngày bắt đầu
        //     if (this.startDate && this.isSameDate(date, this.startDate)) {
        //         cell.className += ' start-date';
        //     }

        //     // Thêm lớp CSS cho ngày kết thúc
        //     if (this.endDate && this.isSameDate(date, this.endDate)) {
        //         cell.className += ' end-date';
        //     }

        //     // Thêm lớp CSS cho các ngày trong khoảng
        //     if (this.startDate && this.endDate &&
        //         this.isDateBetween(date, this.startDate, this.endDate)) {
        //         cell.className += ' in-range';
        //     }

        //     return cell;
        // },

        customCellRenderer(cell) {
            const date = cell.date;
            // Format aria-label in the format YYYY/MM/DD
            const formattedDate = `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()}`;

            // Return an object with the updated attributes
            return {
                attrs: {
                    'aria-label': formattedDate
                }
            };
        },

        // Các phương thức phụ trợ
        customCellContent(cell) {
            console.log("cell: ", cell);
            return {
                class: {
                    nutbd: this.isSameDate(
                        this.formatDate(cell.date),
                        this.formatDate(this.selectedDate)
                    )
                }
            }
        },

        formatDate(date) {
            return new Date(date).toISOString().split('T')[0];
        },

        getCellClasses(cell) {
            const classes = [];
            const cellDate = cell.date;

            if (this.startDate && this.isSameDate(cellDate, new Date(this.startDate))) {
                classes.push('start-date');
            }

            if (this.endDate && this.isSameDate(cellDate, new Date(this.endDate))) {
                classes.push('end-date');
            }

            if (this.startDate && this.endDate) {
                const cellTime = cellDate.getTime();
                const startTime = new Date(this.startDate).getTime();
                const endTime = new Date(this.endDate).getTime();

                if (cellTime > startTime && cellTime < endTime) {
                    classes.push('in-range');
                }
            }

            return classes;
        },

        isSameDate(date1, date2) {
            return date1.getFullYear() === date2.getFullYear() &&
                date1.getMonth() === date2.getMonth() &&
                date1.getDate() === date2.getDate();
        },

        isDateBetween(date, startDate, endDate) {
            return date > startDate && date < endDate;
        }
    },
    async created() {
        await this.getHours();

    }
};
</script>
<style>
.nutbd {
    background-color: #4CAF50 !important;
    color: #F44336 !important;
}

input[type="checkbox"] {
    width: 20px;
    height: 20px;
    cursor: pointer;
    appearance: none;
    /* Xóa mặc định */
    border: 2px solid var(--color-green2);
    background-color: transparent;
    border-radius: 4px;
    /* Làm góc bo tròn nếu cần */
    position: relative;
}

input[type="checkbox"]:checked {
    background-color: var(--color-green2);
    /* Màu nền khi được chọn */
    /* Đường viền */
}

input[type="checkbox"]:checked::before {
    content: "✔";
    /* Biểu tượng check */
    font-size: 14px;
    color: rgb(255, 255, 255);
    /* Màu biểu tượng check */
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
}

.toast {
    /* position: fixed; */
    /* top: 20px;
  right: 20px; */
    z-index: 1050;
}


/* .vuecal__cell--selected {
    background-color: #ffcc00;
    color: white;
    border-radius: 5px 0 0px 5px;
}

.vuecal__cell--selected1 {
    background-color: #ffcc00;
    color: white;
    border-radius: 0 5px 5px 0;
} */


/* Kiểu nền gốc cho các ô */
.vuecal__cell:before {
    content: "";
    position: absolute;
    z-index: 0;
    top: 0;
    left: 0;
    right: 0px;
    bottom: 0px;
    border: 1px solid rgba(196, 196, 196, .25);
}

/* Kiểu Styling cho Ngày Bắt Đầu */
.vuecal__cell.start-date {
    background-color: #4CAF50 !important;
    position: relative;
    z-index: 2;
}

.vuecal__cell.start-date .vuecal__cell-content {
    color: white !important;
    font-weight: bold;
}

.vuecal__cell.start-date::after {
    content: 'Bắt đầu';
    position: absolute;
    bottom: 2px;
    left: 50%;
    transform: translateX(-50%);
    font-size: 10px;
    color: white;
}

/* Kiểu Styling cho Ngày Kết Thúc */
.vuecal__cell.end-date {
    background-color: #F44336 !important;
    position: relative;
    z-index: 2;
}

.vuecal__cell.end-date .vuecal__cell-content {
    color: white !important;
    font-weight: bold;
}

.vuecal__cell.end-date::after {
    content: 'Kết thúc';
    position: absolute;
    bottom: 2px;
    left: 50%;
    transform: translateX(-50%);
    font-size: 10px;
    color: white;
}

/* Kiểu Styling cho Khoảng Ngày Được Chọn */
.vuecal__cell.in-range {
    background-color: #E8F5E9 !important;
    position: relative;
    z-index: 1;
}

.vuecal__cell.in-range::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(76, 175, 80, 0.1);
    pointer-events: none;
}

/* Hiệu Ứng Di Chuột Cho Việc Chọn Khoảng */
.vuecal__cell.hover-range {
    background-color: rgba(76, 175, 80, 0.2) !important;
    transition: background-color 0.2s ease;
}

/* Các Ngày Bị Vô Hiệu Hóa */
.vuecal__cell--disabled {
    text-decoration: line-through;
    background-color: #f5f5f5 !important;
    color: #bdbdbd !important;
    opacity: 0.7;
    cursor: not-allowed;
}

/* Hiệu Ứng Di Chuột */
.vuecal__cell:not(.vuecal__cell--disabled):hover {
    cursor: pointer;
    transition: all 0.2s ease;
    transform: scale(1.05);
}

/* Chuyển Động Của Ô Lịch */
.vuecal__cell {
    transition: all 0.3s ease;
}

/* Tiêu Đề Lịch */
.vuecal__header {
    background-color: #213a58 !important;
    color: white !important;
}
</style>








<!-- <div class="hours">
                    <label for="">Nhận xe</label>
                    <select v-model="selectedStartTime" @click="CheckHours()">
                        <option v-for="(hour, index) in availableHours" :key="index" :value="index"
                            :hidden="startDate == null" :disabled="hour.disabled">
                            {{ hour.name }}
                        </option>
                    </select>
                </div>
                <div>
                    <label for="">Trả xe</label>
                    <select v-model="selectedEndTime" @click="CheckHoursV2()" @change="SumTimes()">
                        <option v-for="(hour, index) in availableHours" :key="index" :value="index"
                            :hidden="endDate == null" :disabled="hour.disabled">
                            {{ hour.name }}
                        </option>
                    </select>
                </div> -->


<!-- Display the selected time -->
<!-- <p>Giờ bắt đầu bạn chọn là: {{ availableHours[selectedStartTime]?.name }}</p> -->




<!-- Display the selected time -->
<!-- <p>Giờ kết thúc bạn chọn là: {{ availableHours[selectedEndTime]?.name }}</p> -->
