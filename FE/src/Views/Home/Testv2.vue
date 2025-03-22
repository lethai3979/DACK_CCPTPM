<template>
    <div>
      <Datepicker 
        style="width: 700px;" 
        v-model="selectedDate"
        :highlighted="highlightedDates"
        :disabled-date="disabledDate"
        @change="onDateChange"
        inline   
      />
      <p>Ngày đã chọn: {{ selectedDate }}</p>
    </div>
  </template>
  
  <script>
  import { ref } from 'vue';
  import Datepicker from 'vue-datepicker-next';
  import '../../../node_modules/vue-datepicker-next/index.css'; // CSS của thư viện này
  
  export default {
    components: {
      Datepicker
    },
    setup() {
      const selectedDate = ref(null); // Ngày được chọn
      const highlightedDates = ref([
        new Date(2024, 11, 25), // Làm nổi bật ngày 25/12/2024
        new Date(2024, 11, 31)  // Làm nổi bật ngày 31/12/2024
      ]);
  
      // Hàm này được gọi khi người dùng thay đổi ngày
      const onDateChange = (date) => {
        console.log('Ngày đã chọn: ', date);
      };
  
      // Hàm này để khóa ngày (khóa ngày 27/12/2024)
      const disabledDate = (date) => {
        const disabledDates = [
          new Date(2024, 12, 27) // Khóa ngày 27/12/2024
        ];
  
        // Kiểm tra nếu ngày có nằm trong danh sách ngày bị khóa không
        return disabledDates.some(disabledDate => 
          disabledDate.getDate() === date.getDate() && 
          disabledDate.getMonth() === date.getMonth() && 
          disabledDate.getFullYear() === date.getFullYear()
        );
      };
  
      return {
        selectedDate,
        highlightedDates,
        onDateChange,
        disabledDate
      };
    }
  };
  </script>
  
  <style scoped>
  /* Tùy chỉnh CSS cho ngày đã chọn và ngày bị khóa */
  .vue-datepicker-next__day--highlighted {
    background-color: #ff6347; /* Màu nền cho ngày được làm nổi bật */
    color: white;
  }
  
  .vue-datepicker-next__day--disabled {
    background-color: #e0e0e0; /* Màu nền cho ngày bị khóa */
    color: #b0b0b0;
  }
  </style>
  