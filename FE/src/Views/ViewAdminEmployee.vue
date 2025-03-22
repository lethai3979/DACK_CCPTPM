<template>
    <div class="infor">
        <div class="sidebar_left">
            <!-- Dành cho admin -->
            <div class="divdetails" v-if="user.roles == 'Admin'">
                <details>
                    <summary>Quản lý bài đăng</summary>
                    <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-postAdmin') }"
                        @click="goToRoute('admin-postAdmin')">
                        Danh sách bài đăng
                    </button>
                    <!-- <button style="margin-top: 0;" :class="{ click: isActiveRoute('user-profile-inforuser') }"
                        @click="goToRoute('user-profile-inforuser')">
                        Bài đăng vi phạm
                    </button> -->
                </details>
                <details>
                    <summary>Quản lý đối tượng</summary>
                    <details>
                        <summary class="child">Quản lý báo cáo</summary>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-reportType-create') }"
                            @click="goToRoute('admin-reportType-create')">
                            Thêm mới loại báo cáo
                        </button>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-reportType') }"
                            @click="goToRoute('admin-reportType')">
                            Danh sách loại báo cáo
                        </button>
                    </details>
                    <details>
                        <summary class="child">Quản lý khuyến mãi</summary>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-promotion-create') }"
                            @click="goToRoute('admin-promotion-create')">
                            Thêm mới khuyến mãi
                        </button>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-promotion') }"
                            @click="goToRoute('admin-promotion')">
                            Danh sách khuyến mãi
                        </button>
                    </details>
                    <details>
                        <summary class="child">Quản lý tiện nghi</summary>

                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-amenity-create') }"
                            @click="goToRoute('admin-amenity-create')">
                            Thêm mới tiện nghi
                        </button>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-amenity') }"
                            @click="goToRoute('admin-amenity')">
                            Danh sách tiện nghi
                        </button>
                    </details>
                    <details>
                        <summary class="child">Quản lý loại xe</summary>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-cartype-create') }"
                            @click="goToRoute('admin-cartype-create')">
                            Thêm mới loại xe
                        </button>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-cartype') }"
                            @click="goToRoute('admin-cartype')">
                            Danh sách loại xe
                        </button>
                    </details>
                    <details>
                        <summary class="child">Quản lý hãng xe</summary>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-company-create') }"
                            @click="goToRoute('admin-company-create')">
                            Thêm mới hãng xe
                        </button>
                        <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-company') }"
                            @click="goToRoute('admin-company')">
                            Danh sách hãng xe
                        </button>
                    </details>
                </details>
                <details>
                    <summary>Thống kê</summary>

                    <p>xin chào</p>
                </details>
                <details>
                    <summary>Quản lý người dùng</summary>
                    <details>
                        <summary style="margin-top: 0;" :class="{ click: isActiveRoute('admin-user') }"
                        @click="goToRoute('admin-user')">Quản lý khách hàng</summary>
                    </details>
                    <details>
                        <summary>Quản lý nhân viên</summary>
                    </details>
                </details>
                <details><summary @click="LogOut()">Đăng xuất</summary></details>
            </div>
            <!-- Danfh cho Employee -->
            <div class="divdetails" v-if="user.roles == 'Employee'">
                <details>
                    <summary>Xử lý yêu cầu</summary>
                    <button style="margin-top: 0;" :class="{ click: isActiveRoute('employee-driver') }"
                        @click="goToRoute('employee-driver')">
                        Quản lý tài xế
                    </button>
                    <button style="margin-top: 0;" :class="{ click: isActiveRoute('employee-getallreport') }"
                        @click="goToRoute('employee-getallreport')">
                        Quản lý vi phạm
                    </button>
                    <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-employee-request') }"
                        @click="goToRoute('admin-employee-request')">
                        Quản lý hủy chuyến
                    </button>
                    <button style="margin-top: 0;" :class="{ click: isActiveRoute('admin-employee-refund') }"
                        @click="goToRoute('admin-employee-refund')">
                        Quản lý trả cọc
                    </button>
                </details>
                <details><summary @click="LogOut()">Đăng xuất</summary></details>

            </div>
        </div>
        <div class="sidebar_right">
            <router-view></router-view>
        </div>
    </div>
</template>

<script>
import UserVM from '../Model/UserVM';
import AuthenticationService from '../Service/api/AuthenticationService';
import TokenService from '../Service/authen/TokenService';
export default {
    data() {
        return {
            page: 0,
            user: new UserVM(),
        }
    },
    methods: {
        async LogOut() {
            const response = await AuthenticationService.Logout();
            const haha = TokenService.removeToken();
            sessionStorage.setItem('LogOut', true);
            const logout = sessionStorage.getItem('LogOut');
            console.log(logout);
            // window.location.reload();
            this.logout = false;
            this.$router.push({ path: '/Home' });
        },
        // Hàm kiểm tra nếu route hiện tại là đường dẫn đã chọn
        isActiveRoute(routeName) {
            return this.$route.name === routeName;
        },
        // Hàm chuyển hướng đến route cụ thể
        goToRoute(routeName) {
            this.$router.push({ name: routeName });
        }
    },
    created() {
        const userString = sessionStorage.getItem('User');
        if(userString !== null){
            this.user = JSON.parse(sessionStorage.getItem('User'));
        }
    },
}
</script>

<style>
.name_img {
    padding: 10px 20px;
    margin-top: 5px;
    font-size: 18px;
    color: #ccc;
}

.infor {
    margin-top: 1px;
    width: 100%;
    display: flex;
    height: 643px;
}

.sidebar_left {
    width: 25%;
}

.sidebar_left button {
    width: 100%;
    text-align: left;
    padding: 15px 10px;
    background-color: #f6f6f6;
    color: #ccc;
    border: none;
}

.sidebar_left button.click {
    background-color: #ccc;
    color: #333;
}

.sidebar_right {
    width: 75%;
    padding: 25px;
    background-color: var(--bg-colorBackground);
    height: 100%;
    overflow-y: auto;
}

.child {
    padding-left: 30px;
}
</style>










































<!-- <template>
    <div class="infor">
        <div class="sidebar_left">

            <div class="name_img">GoWheels Xin chào</div>

            <hr width="100%" style="margin-block: 10px;color: #ccc;">

            <button style="margin-top: 0;" :class="{ click: page == 0 }"
                @click="page = 0; this.$router.push({ name: 'user-profile-inforuser' });">
                Thông tin cá nhân
            </button>
            <button :class="{ click: page == 1 }" 
                @click="page = 1; this.$router.push({ name: 'user-profile-mycar' });">
                Xe cho thuê của bạn
            </button>
            <button :class="{ click: page == 2 }"
                @click="page = 2; this.$router.push({ name: 'user-profile-historyInvoice' });">
                Lịch sử giao dịch
            </button>
            <button :class="{ click: page == 3 }"
                @click="page = 3; this.$router.push({ name: 'user-profile-historyBooking' });">
                Lịch sử đặt xe
            </button>
            <button :class="{ click: page == 4 }"
                @click="page = 4; this.$router.push({ name: 'user-profile-favoriteList' });">
                Danh sách yêu thích
            </button>
            <button :class="{ click: page == 5 }"
                @click="page = 5; this.$router.push({ name: 'user-profile-voucherList' });">
                Danh sách khuyến mãi
            </button>
            <button :class="{ click: page == 6 }"
                @click="page = 6; this.$router.push({ name: 'user-profile-changEmail' });">
                Thay đổi Email
            </button>
            <button :class="{ click: page == 7 }"
                @click="page = 7; this.$router.push({ name: 'user-profile-changPassword' });">
                Thay đổi password
            </button>
            <button :class="{ click: page == 8 }" @click="page = 8; this.$router.push({ name: 'user-profile-mycar' });">
                Yêu cầu xóa tài khoản
            </button>
            <button :class="{ click: page == 9 }">Đăng xuất</button>
        </div>
        <div class="sidebar_right">
            <router-view></router-view>
        </div>
    </div>
</template>

<script>


export default {
    data() {
        return {
            page: 0,
            user: []
        }
    },
    methods: {
    },
    created() {
        const userString = sessionStorage.getItem('User');
        this.user = JSON.parse(userString);
    },

}
</script>

<style>
.name_img {
    padding: 10px 20px;
    margin-top: 5px;
    font-size: 18px;
    color: #ccc;
}

.infor {
    margin-top: 1px;
    width: 100%;
    display: flex;
    height: 643px;
}

.sidebar_left {
    width: 25%;
    /* background-color: #333; */
}

.sidebar_left button {
    width: 100%;
    text-align: left;
    padding: 15px 10px;
    background-color: #f6f6f6;
    color: #ccc;
    /* border: 0.2px solid #ccc; */
    border: none;
    float: left;
}

.sidebar_left router-link {
    width: 100%;
    text-align: left;
    padding: 15px 10px;
    background-color: #f6f6f6;
    color: #ccc;
    /* border: 0.2px solid #ccc; */
    border: none;
    float: left;
}

.sidebar_left button.click {
    background-color: #ccc;
    color: #333;
}

.sidebar_right {
    width: 75%;
    background-color: blue;
    height: 100%;
    overflow-y: auto;
}
</style> -->