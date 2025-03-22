<template>
  <!-- <ViewDemo style="width: 100%;"/> -->
  <!-- <MapV3/> -->
  <!-- v-if="showmess == true" -->
  <div class="toast1 hover" v-if="showmess == true" style="z-index: 99999999999;position: fixed;margin-top: 35px;">
    <div class="toast-header">
      <label class="rounded me-2" style="background-color: #0d6efd;width: 20px;height: 20px;"></label>
      <strong class="me-auto">Thông báo mới</strong>
      <!-- <small class="text-body-secondary">11 mins ago</small> -->
      <button type="button" class="btn-close" @click="showmess = false"></button>
    </div>
    
    <div class="toast-body" v-if="messSignalR == 'You have new booking request'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/You have new booking request.png" alt="">
      Đã có một yêu cầu đặt xe từ khách hàng
    </div>
    <div class="toast-body" v-if="messSignalR == 'Your booking confirmed by owner'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/Your booking confirmed by owner.png" alt="">
      Đơn đặt xe đã được chủ xe chấp nhận
    </div>
    <div class="toast-body" v-if="messSignalR == 'New booking nearby'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/New booking nearby.png" alt="">
      Có đơn đặt xe ở gần khu vực của bạn
    </div>
    <div class="toast-body" v-if="messSignalR == 'Your booking has been denied'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/Your booking has been denied.png" alt="">
      Đơn đặt xe đã bị chủ xe từ chối
    </div>
    <div class="toast-body" v-if="messSignalR == 'The driver has accepted your booking'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/The driver has accepted your booking.png" alt="">
      Đơn đặt xe đã có tài xế chấp nhận
    </div>
    <div class="toast-body" v-if="messSignalR == 'Your selected booking is canceled'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/Your selected booking is canceled logo.png" alt="">
      Khách hàng đã hủy chuyến mà bạn đã nhận
    </div>
    <div class="toast-body" v-if="messSignalR == 'Your booking request has canceled'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/Your booking request has canceled logo.png" alt="">
      Khách hàng đã hủy đơn đặt xe
    </div>
    <div class="toast-body" v-if="messSignalR == 'Your cancellation request has been confirmed'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/Your cancellation request has been confirmed logo.png" alt="">
      Yêu cầu hủy đơn của bạn đã được xác nhận
    </div>
    <div class="toast-body" v-if="messSignalR == 'Your booking driver has cancel'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/Your booking driver has cancel.png" alt="">
      Tài xế của đơn đã hủy nhận chuyến
    </div>
    <div class="toast-body" v-if="messSignalR == 'Your cancellation request has been confirmed'">
      <img width="45px" height="45px" style="margin-right: 10px;border-radius: 50%;"
      src="../src/assets/logoWeb/logomess/Your cancellation request has been confirmed logo.png" alt="">
      Yêu cầu hủy đơn của bạn đã được xác nhận
    </div>

<!-- 
    <div v-if="messSignalR == 'The driver has accepted your booking'" class="toast-body">
      Đơn đặt xe của bạn đã có tài xế
    </div>
    <div v-if="messSignalR == 'You have new booking request'" class="toast-body">
      Bạn có yêu cầu đặt xe mới
    </div>
    <div v-if="messSignalR == 'New booking nearby'" class="toast-body">
      Có yêu cầu đặt xe mới ở gần bạn
    </div>
    <div v-if="messSignalR == 'Your booking confirmed by owner'" class="toast-body">
      Đơn đặt xe của bạn đã được xác nhận
    </div>
    <div v-if="messSignalR == 'Your booking has been denied'" class="toast-body">
      Đơn đặt xe của bạn đã bị từ chối
    </div> -->
  </div>
  <div class="body">
    <header class="d-flex justify-content-between align-items-center p-3 bg-light border-bottom">
      <!-- Logo và menu bên trái -->
      <div class="d-flex align-items-center">
        <a @click="this.$router.push({ path: '/Home' })" class="me-4">
          <img class="logo img-fluid" src="../src/assets/logoWeb/1.png" alt="Logo" style="max-width: 100px;">
        </a>
        <div v-if="user.roles.includes('Driver')" style="display: flex;">
          <button class="btn nutshort" @click="this.$router.push({ name: 'driver-list' });">Đơn đặt
            tài xế
          </button>
          <div class="header_list_child" style="margin-left: 20px;padding: 9px 13px !important;">
            <label style="display: flex;" @change="UpdateLocationByDriver()">
              <input :checked="auto == true" type="checkbox" class="hover" style="margin-top: 2px;margin-right:5px;">
              <i>Tự động cập nhật vị trí</i>
            </label>

          </div>
        </div>

      </div>

      <!-- Nút login bên phải -->
      <div class="d-flex align-items-center">
        <div style="position: relative;margin-right: 30px;margin-top: 5px;" v-if="user.roles.includes('User')">
          <img width="45px" height="45px" src="../src/assets/logoWeb/bell.png" @click="showListMess()" class="chuong" alt="">
          <p v-if="unreadMessages > 0" class="unread">
            <i v-if="unreadMessages <= 9" style="font-size: 11.5px;color: var(--color-white1);margin-top: 1px;">{{
              unreadMessages }}</i>
            <i v-else style="font-size: 11.5px;color: var(--color-white1);margin-top: 1px;">9+</i>
          </p>
          <p v-else class="elseunread">
          </p>
          <div v-if="notify" class="ifnotify">
            <div style="max-height: 500px;overflow-y: auto;padding-top: 10px;">
              <h3>Thông báo</h3>
              <div style="display: flex; margin-bottom: 15px;">
                <button style="padding: 3px 10px;border: none;margin-inline: 10px;border-radius: 30px;"
                  :class="{ success: statusnotify == false }" @click="statusnotify = false">Tất cả</button>
                <button style="padding: 3px 10px;border: none;border-radius: 30px;"
                  :class="{ success: statusnotify == true }" @click="statusnotify = true">Mới</button>
              </div>
              <div v-if="notifies.length > 0">
                <div class="mess hover" style="" v-for="item in notifies" :key="item.id" @click="readMess(item)">
                   <div class="zin" v-if="item.content == 'You have new booking request'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/You have new booking request.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Có yêu cầu đặt xe mới
                      </div>
                      <div class="contentmess">Bạn có một yêu cầu đặt xe từ khách hàng</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'Your booking confirmed by owner'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/Your booking confirmed by owner.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Đơn đã được xác nhận
                      </div>
                      <div class="contentmess">Đơn đặt xe đã được chủ xe chấp nhận</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'New booking nearby'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/New booking nearby.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Có đơn ở gần bạn
                      </div>
                      <div class="contentmess">Có đơn đặt xe ở gần khu vực của bạn</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'Your booking has been denied'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/Your booking has been denied.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Đơn của bạn đã bị từ chối
                      </div>
                      <div class="contentmess">Đơn đặt xe đã bị chủ xe từ chối</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'The driver has accepted your booking'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/The driver has accepted your booking.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Đơn đã có tài xế
                      </div>
                      <div class="contentmess">Đơn đặt xe đã có tài xế chấp nhận</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'Your booking driver has cancel'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/Your booking driver has cancel.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Tài xế đã hủy đơn
                      </div>
                      <div class="contentmess">Tài xế của đơn đã hủy nhận chuyến</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'Your selected booking is canceled'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/Your selected booking is canceled logo.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Khách hàng hủy chuyến
                      </div>
                      <div class="contentmess">Khách hàng đã hủy chuyến mà bạn đã nhận</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'Your booking request has canceled'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/Your booking request has canceled logo.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Khách hàng hủy chuyến
                      </div>
                      <div class="contentmess">Khách hàng đã hủy đơn đặt xe</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'Your cancellation request has been confirmed'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/Your cancellation request has been confirmed logo.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Đã hủy đơn thành công
                      </div>
                      <div class="contentmess">Yêu cầu hủy đơn của bạn đã được xác nhận</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
                  <div class="zin" v-if="item.content == 'Your cancellation request has been denied'">
                    <div class="leftmess">
                      <img width="45px" height="45px"
                        src="../src/assets/logoWeb/logomess/Your cancellation request has been denied logo.png" alt="">
                    </div>
                    <div class="centermess">
                      <div class="titlemess">Hủy đơn thất bại 
                      </div>
                      <div class="contentmess">Yêu cầu hủy đơn của bạn đã bị từ chối</div>
                      <div class="timecreate " :class="{ readf: item.isRead == false }">{{
                        calculateTimeDifference(item.createOn)
                        }}</div>
                    </div>
                    <div class="rightmess">
                      <p :class="{ isReadF: item.isRead == false }"
                        style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;">
                      </p>
                    </div>
                  </div>
<!-- 
                   <div class="leftmess">
                    <img v-if="item.content == 'Your booking confirmed by owner'" width="45px" height="45px"
                      src="../src/assets/logoWeb/coinaccept.png" alt="">
                    <img v-if="item.content == 'Your booking has been denied'" width="45px" height="45px"
                      src="../src/assets/logoWeb/coincancel.png" alt="">
                    <img v-if="item.content == 'New booking nearby' || item.content == 'You have new booking request'"
                      width="45px" height="45px" src="../src/assets/logoWeb/newcar.png" alt="">
                  </div>
                  <div class="centermess" style="">
                    <div class="titlemess" v-if="item.content == 'Your booking confirmed by owner'"
                      style="font-weight: bold;">Đã xác nhận đơn</div>
                    <div class="titlemess" v-if="item.content == 'The driver has accepted your booking'"
                      style="font-weight: bold;">Đơn đã có tài xế</div>
                    <div class="titlemess" v-if="item.content == 'Your booking has been denied'"
                      style="font-weight: bold;">Đã bị từ chối đơn</div>
                    <div class="titlemess" v-if="item.content == 'New booking nearby'" style="font-weight: bold;">Có đơn
                      đặt tài xế</div>
                    <div class="titlemess" v-if="item.content == 'You have new booking request'"
                      style="font-weight: bold;">Đã có đơn đặt xe</div>
                    <div class="contentmess" v-if="item.content == 'Your booking confirmed by owner'"
                      style="font-size: 12px;color: #434343;">Đơn đặt xe đã được chủ xe chấp nhận</div>
                    <div class="contentmess" v-if="item.content == 'The driver has accepted your booking'"
                      style="font-size: 12px;color: #434343;">Đơn đặt xe đã có tài xế</div>
                    <div class="contentmess" v-if="item.content == 'Your booking has been denied'"
                      style="font-size: 12px;color: #434343;">Đơn đặt xe đã bị chủ xe từ chối</div>
                    <div class="contentmess" v-if="item.content == 'New booking nearby'"
                      style="font-size: 12px;color: #434343;">Đã có đơn đặt tài xế gần bạn</div>
                    <div class="contentmess" v-if="item.content == 'You have new booking request'"
                      style="font-size: 12px;color: #434343;">Đã có người đặt xe của bạn</div>
                    <div class="timecreate " :class="{ readf: item.isRead == false }" style="font-size: 11.5px;">{{
                      calculateTimeDifference(item.createOn)
                      }}</div>
                  </div>
                  <div class="rightmess" style="">
                    <p :class="{ isReadF: item.isRead == false }"
                      style="width: 10px; height: 10px;border-radius: 50%;margin-top: 15px;"></p>
                  </div> 
                  <hr> -->
                </div>
              </div>
              <div v-else>
                <div class="mess">
                  <div class="centermess" style="width: 77%; text-align: left;margin: 0 5px;">
                    <div class="titlemess" style="font-weight: bold;">Không có thông báo</div>
                  </div>
                </div>
              </div>
            </div>

          </div>

        </div>
        <button v-if="!logintrue" @click="HamXoaLogin()" class="btn btn-outline-primary">
          <i>Login</i>
        </button>
        <span class="hover" v-if="logintrue" @click="this.$router.push({ name: 'user-profile-inforuser' });">
          <label for="">{{ user.name }}</label>
          <img style="margin-left:10px;border-radius: 50px;object-fit: cover;margin-top: -10px;" width="50px"
            height="50px" class="user-img-home" :src="'http://localhost:5027/' + user?.image" alt="User Avatar" />
        </span>

      </div>
    </header>
    <TestLisst />
    <!-- <Location/> -->

    <div v-if="login == true" class="Login">
      <Login v-if="login == true" @LogOutToApp="LogOutToApp" @HamXoaLogin="HamXoaLogin" @HamGetUser="HamGetUser" />
    </div>
    <div style="background-color: #f6f6f6;">
      <router-view></router-view>
    </div>

  </div>


</template>
<script setup>
import { ref, provide, onMounted, watch } from 'vue';
import axios from 'axios';
import { useRoute } from 'vue-router';
import InfoVue from './components/Info.vue';
import Login from './Views/Authen/Login.vue';

// Mở thông tin người dùng
const open = ref(false);

const showDrawer = () => {
  open.value = true;
};



const route = useRoute()
const carTypes = ref([])
const amenities = ref([])
const companies = ref([])
const isLoading = ref(false)

provide('carTypes', carTypes)
provide('amenities', amenities)
provide('companies', companies)

const LogOut = async () => {
  const logout = sessionStorage.getItem('LogOut');
  console.log(logout);
  if (logout != null) {
    window.location.reload();
    startSignalRConnection();
    sessionStorage.removeItem('LogOut');

  }
  // const response = await AuthenticationService.Logout();
  // TokenService.removeToken();
  // // window.location.reload();
  // this.$router.push({ path: '/Home' });
}
const GetAllCarTypes = async () => {
  try {
    const response = await axios.get('http://localhost:5027/api/admin/Cartype/GetAll')
    if (response.status === 200 && response.data.data) {
      carTypes.value = response.data.data
      // console.log('Car Types:', carTypes.value)
    } else {
      carTypes.value = []
      console.log('Không có dữ liệu car types')
    }
  } catch (error) {
    console.error('Lỗi lấy dữ liệu car types:', error)
    carTypes.value = []
  }
}

const GetAllAmenities = async () => {
  try {
    const response = await axios.get('http://localhost:5027/api/admin/Amenity/GetAll')
    if (response.status === 200 && response.data.data) {
      amenities.value = response.data.data
      // console.log('Amenities:', amenities.value)
    } else {
      amenities.value = []
      console.log('Không có dữ liệu amenities')
    }
  } catch (error) {
    console.error('Lỗi lấy dữ liệu amenities:', error)
    amenities.value = []
  }
}
const GetAllCompanies = async () => {
  try {
    const response = await axios.get('http://localhost:5027/api/admin/Company/GetAll')
    if (response.status === 200 && response.data.data) {
      companies.value = response.data.data
      // console.log('Companies:', companies.value)
    } else {
      companies.value = []
      console.log('Không có dữ liệu companies')
    }
  } catch (error) {
    console.error('Lỗi lấy dữ liệu companies:', error)
    companies.value = []
  }
}
const fetchAllData = async () => {
  console.log('Fetching all data...')
  isLoading.value = true
  try {
    await Promise.all([
      GetAllCarTypes(),
      GetAllAmenities(),
      GetAllCompanies()
    ])
    console.log('All data fetched successfully')
  } catch (error) {
    console.error('Error fetching data:', error)
  } finally {
    isLoading.value = false
  }
}

watch(
  () => route.path,
  async (newPath, oldPath) => {
    if (newPath !== oldPath) {
      console.log('Route changed to:', newPath)

      // Chỉ fetch data cần thiết dựa trên route
      if (newPath.includes('/Home')) {
        await LogOut()
      }
      else if (newPath.includes('/admin/company')) {
        await GetAllCompanies()
      } else if (newPath.includes('/admin/car-types')) {
        await GetAllCarTypes()
      } else if (newPath.includes('/admin/amenities')) {
        await GetAllAmenities()
      } else {
        // Fetch tất cả nếu cần
        await fetchAllData()
      }
    }
  }

)


onMounted(async () => {
  await fetchAllData()
})
</script>
<script>

import Location from './Views/Home/Location.vue';
import SignalR from './Views/Home/SignalR.vue';
import TestLisst from './Views/TestLisst.vue';
import AuthenticationService from './Service/api/AuthenticationService';
import { ref, provide, onMounted } from 'vue';
import axios from 'axios';
import UserVM from './Model/UserVM';
import NotifyVM from './Model/NotifyVM';

import * as signalR from "@microsoft/signalr";
import NotifyService from './Service/api/NotifyService';
import DriverBookingService from './Service/api/DriverBookingService';
import MapV2 from './Views/Home/MapV2.vue';
import MapV3 from './Views/Home/MapV3.vue';
import ViewDemo from './Views/Home/ViewDemo.vue';
export default {
  components: {
    TestLisst, Location, SignalR, MapV2, MapV3, ViewDemo
  },
  watch: {
    login(newVal) {
      // Thay đổi class của body khi login thay đổi
      if (newVal == true) {
        document.body.classList.add("no-scroll");
      } else {
        document.body.classList.remove("no-scroll");
      }
    },
  },
  data() {
    return {
      isLoggingOut: false,
      login: false,
      logintrue: false,
      user: new UserVM(),
      nozTB: false,
      message: "",
      role: '',
      notify: false,
      connection: null,
      unreadMessages: 0,
      notifies: [],
      notifiesisRead: [new NotifyVM()],
      statusnotify: false,
      showmess: false,
      lat: null,
      lng: null,
      messSignalR: '',
      lookNotify: false,
      location: {
        lat: 0,
        lng: 0
      },
      auto: false
    };
  },
  methods: {
    close() {
      this.show = false;
    },
    showListMess() {
      this.notify = !this.notify;
      this.getAllNotify();
      this.showmess = false;
    },
    async getAllNotify() {
      const response = await NotifyService.getAllNotify();
      this.notifies = response.data;
      this.notifies.sort((a, b) => b.id - a.id);
      console.log("Notify đã get");
      await this.FindMess(this.notifies);
    },
    async FindMess(notify) {
      this.unreadMessages = 0;
      console.log("Notify đc lấy ra: ", notify);
      if (notify.length > 0) {
        notify.forEach(element => {
          if (element.isRead == false) {
            this.unreadMessages++;
          }
        })
      }
    },
    async readMess(notify) {
      this.showmess = false;
      this.unreadMessages--;
      if (notify.isRead == false) {
        await this.NotifyReadOnServer(notify.id);
      }
      if (notify.content == 'Your booking confirmed by owner'|| notify.content == 'Your booking has been denied' || notify.content == 'The driver has accepted your booking' || notify.content == 'Your booking driver has cancel' || notify.content == 'Your selected booking is canceled' || notify.content == 'Your booking request has canceled' || notify.content == 'Your cancellation request has been confirmed' || notify.content == 'Your cancellation request has been denied' ) {
        this.$router.push({ name: 'user-booking-detail', params: { id: notify.bookingId } });
      }
      else if (notify.content == 'New booking nearby') {
        this.$router.push({ name: 'driver-list' });
      }
      else if (notify.content == 'You have new booking request') {
        this.$router.push({ name: 'user-profile-mycar' });
        this.$router.push({ name: 'user-booking-detail', params: { id: notify.bookingId } });
      }
      await NotifyService.ReadMesss(notify.id);
      this.notify = false;
    },



    async getUserLocation() {
      // Kiểm tra xem trình duyệt có hỗ trợ Geolocation API không
      if (!navigator.geolocation) {
        this.error = "Geolocation is not supported by your browser.";
        return;
      }

      // Yêu cầu vị trí từ người dùng
      navigator.geolocation.getCurrentPosition(
        async (position) => {
          const { latitude, longitude } = position.coords;
          this.lat = latitude;
          this.lng = longitude;
          const res = await DriverBookingService.UpdateLocation(latitude, longitude);
          this.error = null;
          this.location = {
            lat: latitude,
            lng: longitude
          };
          // this.location = {
          //   lat: 10.8040619,
          //   lng: 106.7129399
          // };
          sessionStorage.setItem('Location', JSON.stringify(this.location));
          console.log("Res của UpdateLocation: ", this.location.lat, this.location.lng);
        },
        (err) => {
          // Xử lý lỗi nếu người dùng từ chối cấp quyền
          switch (err.code) {
            case err.PERMISSION_DENIED:
              this.error = "Permission denied. Please enable location access.";
              break;
            case err.POSITION_UNAVAILABLE:
              this.error = "Position unavailable.";
              break;
            case err.TIMEOUT:
              this.error = "Request timed out.";
              break;
            default:
              this.error = "An unknown error occurred.";
          }
        }
      );
    },
    UpdateLocationByDriver() {
      this.auto = !this.auto;
      const abc = this;
      if (this.auto) {
        this.intervalId = setInterval(() => {
          abc.getUserLocation();
        }, 1800000);
      } else {
        clearInterval(this.intervalId);
      }
    },

    async HamGetUser() {
      const response = await AuthenticationService.getUser();
      this.user = response;
      this.role = this.user.roles;
      this.startSignalRConnection();
      this.getAllNotify();
      if (this.user.roles.includes("Driver")) {
        await this.getUserLocation();
      }
      if (this.user.roles == "User" || this.user.roles == "Driver") {
        this.$router.push({ path: '/Home' });
      }
      if (this.user.roles == "Admin") {
        this.$router.push({ path: '/admin' });
      }
      if (this.user.roles == "Employee") {
        this.$router.push({ path: '/employee' });
      }
      sessionStorage.setItem('User', JSON.stringify(this.user));
      this.GetUser();
    },

    async GetUser() {
      const response = await AuthenticationService.getUser();
      this.logintrue = true;
      this.login = false;
      this.user = response;
      this.role = this.user.roles;
      if (this.user.roles.includes("Driver")) {
        await this.getUserLocation();
      }
      console.log("User đăng nhập: ", this.user);
      console.log("Role hiện tại: ", this.user.role);
      sessionStorage.setItem('User', JSON.stringify(this.user));
    },
    HamXoaLogin() {
      if (this.login == true) {
        this.login = false;
        console.log(this.login);
      }
      else {
        this.login = true;
        console.log(this.login);
      }
    },
    calculateTimeDifference(inputDate) {
      const now = new Date(); // Thời gian hiện tại
      const inputTime = new Date(inputDate); // Chuyển đổi input thành đối tượng Date
      const diffInMs = now - inputTime; // Sự khác biệt thời gian (ms)

      if (diffInMs < 0) {
        return "Thời gian đầu vào là tương lai!";
      }

      const diffInSeconds = Math.floor(diffInMs / 1000);
      if (diffInSeconds < 60) {
        return `${diffInSeconds} giây trước`;
      }

      const diffInMinutes = Math.floor(diffInSeconds / 60);
      if (diffInMinutes < 60) {
        return `${diffInMinutes} phút trước`;
      }

      const diffInHours = Math.floor(diffInMinutes / 60);
      if (diffInHours < 24) {
        return `${diffInHours} giờ trước`;
      }

      const diffInDays = Math.floor(diffInHours / 24);
      if (diffInDays === 1) {
        return `1 ngày trước`;
      } else if (diffInDays < 7) {
        return `${diffInDays} ngày trước`;
      }

      const diffInWeeks = Math.floor(diffInDays / 7);
      if (diffInWeeks === 1) {
        return `1 tuần trước`;
      } else if (diffInWeeks < 4) {
        return `${diffInWeeks} tuần trước`;
      }

      const diffInMonths = Math.floor(diffInDays / 30); // Xấp xỉ 30 ngày = 1 tháng
      if (diffInMonths === 1) {
        return `1 tháng trước`;
      } else if (diffInMonths < 12) {
        return `${diffInMonths} tháng trước`;
      }

      const diffInYears = Math.floor(diffInDays / 365); // Xấp xỉ 365 ngày = 1 năm
      if (diffInYears === 1) {
        return `1 năm trước`;
      } else {
        return `${diffInYears} năm trước`;
      }
    },
    async startSignalRConnection() {
      const token = sessionStorage.getItem('authToken');
      if (!token) {
        console.error('No authentication token found');
        return;
      }
      try {
        this.connection = new signalR.HubConnectionBuilder()
          .withUrl("http://localhost:5027/notifyhub", {
            accessTokenFactory: () => token.replace("Bearer ", "")
          })
          .withAutomaticReconnect([0, 0, 0])
          .build();
        // Handle receiving notifications

        this.connection.on("ReceiveMessage", (mess) => {
          console.log("Received notification:", mess);
          this.notify = false;
          this.handleNewNotification(mess);
        });
        console.log("Received notification dhvshjvhbjdsvhdsbjvdjs:");
        await this.connection.start();
        console.log("SignalR Connected successfully");
      } catch (err) {
        console.error("SignalR Connection Error:", err);
      }
    },

    handleNewNotification(mess) {
      this.showmess = true;
      this.messSignalR = mess;
      setTimeout(() => {
        this.showmess = false;
      }, 25000);
      this.getAllNotify();
    },


    async stopSignalRConnection() {
      if (this.connection) {
        try {
          await this.connection.stop();
          console.log("SignalR Connection Stopped");
        } catch (err) {
          console.error("Error stopping SignalR connection:", err);
        }
      }
    },

    // markNotificationAsRead(notificationId) {
    //   const notification = this.notifies.find(n => n.id === notificationId);
    //   if (notification) {
    //     notification.isRead = true;
    //     this.unreadMessages = Math.max(0, this.unreadMessages - 1);

    //     // Optional: Call API to mark as read on server
    //     this.markNotificationReadOnServer(notificationId);
    //   }
    // },

    async NotifyReadOnServer(notificationId) {
      try {
        // Implement API call to mark notification as read
        await axios.post(`http://localhost:5027/api/Notify/MarkAsRead/${notificationId}`, {}, {
          headers: {
            'Authorization': `Bearer ${sessionStorage.getItem('authToken')}`
          }
        });
      } catch (error) {
        console.error('Failed to mark notification as read:', error);
      }
    }

  },
  created() {
    const logout = sessionStorage.getItem('LogOut');
    if (logout != null) {
      this.stopSignalRConnection();
      sessionStorage.removeItem('LogOut');
    }
    const userString = sessionStorage.getItem('authToken');
    if (userString == null) {
      console.log("Chưa đăng nhập");
    }
    else {
      console.log("1");
      this.getAllNotify();
      this.GetUser();

    }

  },

  mounted() {
    this.startSignalRConnection();
    this.getAllNotify();
  },
  beforeUnmount() {
    this.stopSignalRConnection();
  }
};
</script>
<style></style>
